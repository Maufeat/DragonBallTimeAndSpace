using System;
using System.Collections.Generic;

public interface IWordFilter
{
    void AddKey(string key);
    bool HasBadWord(string text);
    string FindOne(string text);
    List<string> FindAll(string text);
    string Replace(string text);
}

public class TrieFilter : TrieNode, IWordFilter
{
    public void AddKey(string key)
    {
        if (string.IsNullOrEmpty(key))
        {
            return;
        }
        TrieNode trieNode = this;
        for (int i = 0; i < key.Length; i++)
        {
            char simp = CharConverSelf.GetSimp(key[i]);
            trieNode = trieNode.Add(simp);
        }
        trieNode.m_end = true;
    }

    public bool HasBadWord(string text)
    {
        for (int i = 0; i < text.Length; i++)
        {
            int num = i;
            TrieNode trieNode = this;
            while (trieNode.TryGetValue(text[num], out trieNode))
            {
                if (trieNode.m_end)
                {
                    return true;
                }
                if (text.Length == ++num)
                {
                    break;
                }
            }
        }
        return false;
    }

    public string FindOne(string text)
    {
        for (int i = 0; i < text.Length; i++)
        {
            int num = i;
            TrieNode trieNode = this;
            while (trieNode.TryGetValue(CharConverSelf.GetSimp(text[num]), out trieNode))
            {
                if (trieNode.m_end)
                {
                    return text.Substring(i, num - i + 1);
                }
                if (text.Length == ++num)
                {
                    break;
                }
            }
        }
        return string.Empty;
    }

    public List<string> FindAll(string text)
    {
        List<string> list = new List<string>();
        for (int i = 0; i < text.Length; i++)
        {
            int num = i;
            TrieNode trieNode = this;
            while (trieNode.TryGetValue(CharConverSelf.GetSimp(text[num]), out trieNode))
            {
                if (trieNode.m_end)
                {
                    list.Add(text.Substring(i, num - i + 1));
                }
                if (text.Length == ++num)
                {
                    break;
                }
            }
        }
        return list;
    }

    public string Replace(string text)
    {
        char[] array = null;
        for (int i = 0; i < text.Length; i++)
        {
            int num = i;
            TrieNode trieNode = this;
            while (trieNode.TryGetValue(CharConverSelf.GetSimp(text[num]), out trieNode))
            {
                if (trieNode.m_end)
                {
                    if (array == null)
                    {
                        array = text.ToCharArray();
                    }
                    for (int j = i; j <= num; j++)
                    {
                        array[j] = this.Mask;
                    }
                    i = num;
                }
                if (text.Length == ++num)
                {
                    break;
                }
            }
        }
        return (array != null) ? new string(array) : text;
    }

    public char Mask = '*';
}
