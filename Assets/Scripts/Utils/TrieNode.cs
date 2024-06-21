using System;
using System.Collections.Generic;

public class TrieNode
{
    public TrieNode()
    {
        this.m_values = new Dictionary<char, TrieNode>();
    }

    public bool TryGetValue(char c, out TrieNode node)
    {
        return this.m_values.TryGetValue(c, out node);
    }

    public TrieNode Add(char c)
    {
        TrieNode trieNode;
        if (!this.m_values.TryGetValue(c, out trieNode))
        {
            trieNode = new TrieNode();
            this.m_values.Add(c, trieNode);
        }
        return trieNode;
    }

    public bool m_end;

    public Dictionary<char, TrieNode> m_values;
}
