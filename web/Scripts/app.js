var frequencyDictionaries = {
    'en': {
        'E': 0.127,
        'T': 0.0906,
        'A': 0.0817,
        'O': 0.0751,
        'I': 0.0697,
        'N': 0.0675,
        'S': 0.0633,
        'H': 0.0609,
        'R': 0.0599,
        'D': 0.0425,
        'L': 0.0403,
        'C': 0.0278,
        'U': 0.0276,
        'M': 0.0241,
        'W': 0.0236,
        'F': 0.0223,
        'G': 0.0202,
        'Y': 0.0197,
        'P': 0.0193,
        'B': 0.0149,
        'V': 0.0098,
        'K': 0.0077,
        'X': 0.0015,
        'J': 0.0015,
        'Q': 0.0010,
        'Z': 0.0005
    },
    'ru': {

    }
};

Vue.component('encode', {
    props: ['lang'],
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
                        <h4>Результат обрабтки:</h4>
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
                    alphabet: _.join(_.keys(frequencyDictionaries[self.lang]), ''),
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
    props: ['text', 'lang'],
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
                                        <th>Длина ключевого слова</th>
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
                        <h4>Результат обрабтки:</h4>
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
                    alphabet: _.join(_.keys(frequencyDictionaries[self.lang]), '')
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
                        alphabet: _.join(_.keys(frequencyDictionaries[self.lang]), '')  
                    }
                }).then(function (resp) {
                    self.kasiskiResult = resp;
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

Vue.component('hack', {
    props: ['text', 'lang'],
    data: function () {
        return {
            resultText: null,
            keyword: null,
            inputText: null,
            kasiskiResult: []
        };
    },
    template: `<div class="container">
                    <div class="row">
                        <div class="vigenere-input-data col-md-6 col-sm-6 container">
                            <div class="row">
                                <div class="input-group">
                                    <span class="input-group-btn">
                                    <button class="btn btn-default" type="button" v-on:click="loadKeywords()">Дешифровать</button>
                                    </span>
                                </div>
                            </div>
                            <div class="row">
                                <span>Текст для обрабтки:</span>
                                <textarea class="vigenere-input" v-model="inputText"></textarea>
                            </div>
                        </div>
                        <div class="kasiski-result col-md-6 col-sm-6">
                            <h3>Метод Касиски</h3>
                            <table class="table table-striped">
                                <thead>
                                    <tr>
                                        <th>Длина ключевого слова</th>
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
        loadKeywords: function () {
            let self = this;
            self.$parent.$emit('loading');
            $.ajax('api/vigenere/kasiski', {
                type: 'POST',
                data: {
                    text: self.inputText,
                    alphabet: _.join(_.keys(frequencyDictionaries[self.lang]), '')
                }
            }).then(function (resp) {
                self.kasiskiResult = resp;
                self.keyword = self.kasiskiResult[0];
                let length = self.keyword.size;
                return $.ajax('api/vigenere/get_keywords', {
                    type: 'POST',
                    data: {
                        text: self.inputText,
                        alphabet: _.join(_.keys(frequencyDictionaries[self.lang]), ''),
                        length: length,
                        frequency: _.values(frequencyDictionaries[self.lang])
                    }
                });
            }).then(function (keyword) {
                this.keyword = keyword;
            }).always(function () {
                self.$parent.$emit('loading');
            });
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
                        alphabet: _.join(_.keys(frequencyDictionaries[self.lang]), '')
                    }
                }).then(function (resp) {
                    self.kasiskiResult = resp;
           
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
        preloader: false,
        lang: 'en'
    },
    mounted: function () {
        this.$on('loading', function () {
            this.preloader = !this.preloader;
        });
    }
});
