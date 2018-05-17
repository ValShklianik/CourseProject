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
                Text text = db.Texts.FirstOrDefault(t => t.Value == initText);
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

        public EncodedText GetEncodedText(string initText, string keyword)
        {
            using (var db = new Context())
            {
                EncodedText encoded = db.EncodedTexts.FirstOrDefault(t => t.InitialText.Value == initText && t.Keyword == keyword);
                return encoded;
            }
        }

        public Text AddDecodedText(string initText, string decodedText, string keyword)
        {
            using (var db = new Context())
            {
                Text text = db.Texts.FirstOrDefault(t => t.Value == initText);
                if (ReferenceEquals(text, null)) text = db.Texts.Create();
                text.Value = initText;
                db.SaveChanges();
                EncodedText encoded = db.EncodedTexts.FirstOrDefault(t => t.Value == initText && t.Keyword == keyword);
                if (ReferenceEquals(encoded, null)) encoded = db.EncodedTexts.Create();
                encoded.InitialTextId = text.Id;
                encoded.InitialText = text;
                encoded.Keyword = keyword;
                encoded.Value = decodedText;
                return encoded.InitialText;
            }
        }

        public Text GetDecodedText(string encodedText, string keyword)
        {
            using (var db = new Context())
            {
                EncodedText encoded = db.EncodedTexts.FirstOrDefault(text => text.Value == encodedText && text.Keyword == keyword);

                if (ReferenceEquals(encoded, null)) return null;
                return encoded.InitialText;
            }
        }

        public IEnumerable<KasiskiResultItem> AddKasiskiResult(string text, IEnumerable<Tuple<int, double>> results)
        {
            using (var db = new Context())
            {
                EncodedText encoded = db.EncodedTexts.FirstOrDefault(t => t.Value == text);
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
                var items = new List<KasiskiResultItem>();
                foreach (var res in results)
                {
                    var r = db.KasiskiResultItems.Add(new KasiskiResultItem()
                    {
                        Size = res.Item1,
                        Probability = res.Item2,
                        KasiskiResultField = result,
                        KasiskiResultId = result.Id
                    });
                    db.SaveChanges();
                    items.Add(r);
                }
                result.Results = items;
                db.SaveChanges();
                return items;
            }
        }

        public IEnumerable<KasiskiResultItem> GetKasiskiResult(string text)
        {
            using (var db = new Context())
            {
                var result = db.KasiskiResultItems.Where(res => res.KasiskiResultField.EncodedTextField.Value == text);
                if (ReferenceEquals(result, null)) return null;
                if (result.Count(r => true) == 0) return null;
                return result.ToList();
            }
        }
    }
}
