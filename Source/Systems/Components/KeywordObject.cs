using System.Collections.Generic;

namespace EndlessSpace
{
    public interface IKeywordObject
    {
        public void AddKeyword(string keyword);
        public void RemoveKeyword(string keyword);
        public bool HasKeyword(string keyword);
        public bool HasKeyword(string[] keywords);
    }

    public class KeywordObject : IKeywordObject
    {
        public readonly List<string> keyword_list = new List<string>();

        public void AddKeyword(string keyword) => keyword_list.Add(keyword);
        public void RemoveKeyword(string keyword) => keyword_list.Remove(keyword);
        public bool HasKeyword(string keyword) => keyword_list.Contains(keyword);
        public bool HasKeyword(string[] keyword)
        {
            foreach (string k in keyword)
            {
                if (keyword_list.Contains(k)) return true;
            }
            return false;
        }
    }
}
