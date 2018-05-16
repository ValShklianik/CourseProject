using DTO.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DAL
{
    public class Repository
    {
        public EncodedText AddEncodedText(string initText, string encodedText, string keyword)
        {
            using (var db = new Context())
            {
                Text text = db.Texts.First(t => t.Value == initText);
                if (ReferenceEquals(text, null)) text = db.Texts.Add(new Text()
                {
                    Value = initText
                });
                db.SaveChanges();

                EncodedText encoded = db.EncodedTexts.Add(new EncodedText() {
                    InitialTextId = text.Id,
                    InitialText = text,
                    Value = encodedText,
                    Keyword = keyword
                });
                db.SaveChanges();
                return encoded;
            }
        }

        public EncodedText GetEncodedText(string text, string keyword)
        {
            using (var db = new Context())
            {
                EncodedText encoded = db.EncodedTexts.First(t => t.InitialText.Value == text && t.Keyword == keyword);

                if (ReferenceEquals(encoded, null)) return null;
                return encoded;
            }
        }

        public Text GetDecodedText(string encodedText, string keyword)
        {
            using (var db = new Context())
            {
                EncodedText encoded = db.EncodedTexts.First(text => text.Value == encodedText && text.Keyword == keyword);

                if (ReferenceEquals(encoded, null)) return null;
                return encoded.InitialText;
            }
        }

        public KasiskiResult AddKasiskiResult(string text, IEnumerable<Tuple<int, double>> results)
        {
            using (var db = new Context())
            {
                EncodedText encoded = db.EncodedTexts.First(t => t.Value == text);
                if (ReferenceEquals(encoded, null)) encoded = db.EncodedTexts.Add(new EncodedText()
                {
                    Value = text
                });
                db.SaveChanges();
                KasiskiResult result = db.KasiskiResults.Add(new KasiskiResult() {
                    EncodedTextId = encoded.Id,
                    EncodedTextField = encoded
                });
                db.SaveChanges();
                result.Results = results.Select(res => db.KasiskiResultItems.Add(new KasiskiResultItem() {
                    Size = res.Item1,
                    Probability = res.Item2
                }));
                db.SaveChanges();
                return result;
            }
        }

        public KasiskiResult GetKasiskiResult(string text)
        {
            using (var db = new Context())
            {
                return db.KasiskiResults.First(res => res.EncodedTextField.Value == text);
            }
        }
    }
}
