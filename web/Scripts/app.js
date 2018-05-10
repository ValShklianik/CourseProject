var app = new Vue({
    el: '#app',
    data: {
        initialText: '',
        encodedText: '',
        keyword: ''
    },
    methods: {
        encode: function (text, keyword) {
            var self = this;
 
            $.ajax('api/vigenere/encode', {
                type: 'GET',
                data: {
                    text: text,
                    keyword: keyword
                }
            }).then(function (resp) {
                self.encodedText = resp;
            });
        },
        kasiski: function (text) {

        }
    }
});