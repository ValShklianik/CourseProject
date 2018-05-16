var russianAlphabed = 'АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ';

Vue.component('encode', {
    data: function () {
        return {
            resultText: null,
            keyword: null,
            inputText: null
        };
    },
    template: `<div class="container">
                <div class="row">
                    <div class="vigenere-input-data col-md-6 col-sm-6 container">
                        <div class="row">
                            <div class="input-group">
                            <input type="text" class="form-control" placeholder="Ключевое слово" v-model="keyword">
                            <span class="input-group-btn">
                            <button class="btn btn-default" type="button" v-on:click="crypt(inputText, keyword)">Шифровать</button>
                            </span>
                        </div>
                        </div>
                        <div class="row">
                            <span>Текст для обрабтки:</span>
                            <textarea class="vigenere-input" v-model="inputText"></textarea>
                        </div>
                    </div>
                    <div class="vigenere-result col-md-6 col-sm-6">
                        <h4>Реультат обрабтки:</h4>
                        <p>{{resultText}}</p>
                    </div>
                </div>
            </div>`,
    methods: {
        crypt: function (text, keyword) {
            var self = this;

            self.$parent.$emit('loading');
            $.ajax('api/vigenere/encode', {
                type: 'POST',
                data: {
                    text: text,
                    keyword: keyword,
                    alphabet: russianAlphabed
                }
            }).then(function (resp) {
                self.resultText = resp;
                self.$root.$data.encodedText = resp;
            }).always(function () {
                self.$parent.$emit('loading');
            });;
        }
    }
});

Vue.component('decode', {
    props: ['text'],
    data: function () {
        return {
            resultText: null,
            keyword: null,
            inputText: null,
            kasiskiResult: []
        };
    },
    template:  `<div class="container">
                    <div class="row">
                        <div class="vigenere-input-data col-md-6 col-sm-6 container">
                            <div class="row">
                                <div class="input-group">
                                    <input type="text" class="form-control" placeholder="Ключевое слово" v-model="keyword">
                                    <span class="input-group-btn">
                                        <button class="btn btn-default" type="button" v-on:click="crypt(inputText, keyword)">Дешифровать</button>
                                    </span>
                                </div>
                            </div>
                            <div class="row">
                                <span>Текст для обрабтки:</span>
                                <textarea class="vigenere-input" v-model="inputText" v-on:blur="loadKasiski(inputText)"></textarea>
                            </div>
                        </div>
                        <div class="kasiski-result col-md-6 col-sm-6">
                            <h3>Метод Касиски</h3>
                            <table class="table table-striped">
                                <thead>
                                    <tr>
                                        <th>Длиа ключевого слова</th>
                                        <th>Вероятность</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr v-for="word in kasiskiResult">
                                        <td>{{word.size}}</td>
                                        <td>{{word.probability}}</td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </div>
                    <div class="vigenere-result row">
                        <h4>Реультат обрабтки:</h4>
                        <p>{{resultText}}</p>
                    </div>
                </div>`,
    methods: {
        crypt: function (text, keyword) {
            var self = this;
            self.$parent.$emit('loading');
            $.ajax('api/vigenere/decode', {
                type: 'POST',
                data: {
                    text: text,
                    keyword: keyword,
                    alphabet: russianAlphabed
                }
            }).done(function (resp) {
                self.resultText = resp;
            }).always(function () {
                self.$parent.$emit('loading');
            });;
        },
        loadKasiski: function (text) {
            var self = this;

            if (!_.isNil(text) && text != '') {
                self.$parent.$emit('loading');
                self.inputText = text;
                $.ajax('api/vigenere/kasiski', {
                    type: 'POST',
                    data: {
                        text: self.inputText,
                        alphabet: russianAlphabed
                    }
                }).then(function (resp) {
                    self.kasiskiResult = resp;
                    let length = _.first(resp).size;
                    return $.ajax('api/vigenere/get_keyword', {
                        type: 'POST',
                        data: {
                            text: self.inputText,
                            alphabet: russianAlphabed,
                            length: length,
                            mostPopular: 'О'
                        }
                    });
                }).then(function (keyword) {
                    this.keyword = keyword;
                }).always(function () {
                    self.$parent.$emit('loading');
                });
            }
        }
    },
    mounted: function () {
        this.loadKasiski(this.text);
    }
});

var app = new Vue({
    el: '#app',
    data: {
        initialText: '',
        encodedText: '',
        keyword: '',
        activeMode: 'encode_mode',
        preloader: false
    },
    mounted: function () {
        this.$on('loading', function () {
            this.preloader = !this.preloader;
        });
    }
});
