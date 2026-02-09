using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace L9;
public class ImageSet : ISet<Image>
{
    private readonly LinkedList<Image> _list = new();
    
    bool ICollection<Image>.IsReadOnly => false;
    
    public int Count => _list.Count;
    
    public bool Add(Image item)
    {
        if (item == null) throw new ArgumentNullException(nameof(item));
        
        if (_list.Contains(item))
        {
            return false;
        }
        
        _list.AddLast(item);
        return true;
    }
    
    void ICollection<Image>.Add(Image item)
    {
        Add(item);
    }

    public void Clear() => _list.Clear();

    public bool Contains(Image item) => _list.Contains(item);

    public void CopyTo(Image[] array, int arrayIndex) => _list.CopyTo(array, arrayIndex);

    public bool Remove(Image item) => _list.Remove(item);

    public IEnumerator<Image> GetEnumerator() => _list.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    
    #region ISet Implementation
    
    public void ExceptWith(IEnumerable<Image> other)
    {
        if (other == null) throw new ArgumentNullException(nameof(other));
        foreach (var item in other)
        {
            Remove(item);
        }
    }

    public void IntersectWith(IEnumerable<Image> other)
    {
        if (other == null) throw new ArgumentNullException(nameof(other));
        var otherSet = new HashSet<Image>(other);
        var toRemove = new List<Image>();
        foreach (var item in this)
        {
            if (!otherSet.Contains(item))
            {
                toRemove.Add(item);
            }
        }
        foreach (var item in toRemove)
        {
            Remove(item);
        }
    }

    public bool IsProperSubsetOf(IEnumerable<Image> other)
    {
        if (other == null) throw new ArgumentNullException(nameof(other));
        var otherSet = new HashSet<Image>(other);
        return this.Count < otherSet.Count && IsSubsetOf(otherSet);
    }

    public bool IsProperSupersetOf(IEnumerable<Image> other)
    {
        if (other == null) throw new ArgumentNullException(nameof(other));
        var otherAsCollection = other.ToList();
        return this.Count > otherAsCollection.Count && IsSupersetOf(otherAsCollection);
    }

    public bool IsSubsetOf(IEnumerable<Image> other)
    {
        if (other == null) throw new ArgumentNullException(nameof(other));
        var otherSet = new HashSet<Image>(other);
        foreach (var item in this)
        {
            if (!otherSet.Contains(item))
            {
                return false;
            }
        }
        return true;
    }

    public bool IsSupersetOf(IEnumerable<Image> other)
    {
        if (other == null) throw new ArgumentNullException(nameof(other));
        foreach (var item in other)
        {
            if (!this.Contains(item))
            {
                return false;
            }
        }
        return true;
    }

    public bool Overlaps(IEnumerable<Image> other)
    {
        if (other == null) throw new ArgumentNullException(nameof(other));
        foreach (var item in other)
        {
            if (this.Contains(item))
            {
                return true;
            }
        }
        return false;
    }

    public bool SetEquals(IEnumerable<Image> other)
    {
        if (other == null) throw new ArgumentNullException(nameof(other));
        var otherSet = new HashSet<Image>(other);
        return this.Count == otherSet.Count && IsSubsetOf(otherSet);
    }

    public void SymmetricExceptWith(IEnumerable<Image> other)
    {
        if (other == null) throw new ArgumentNullException(nameof(other));
        var otherSet = new HashSet<Image>(other);
        var toAdd = new List<Image>();
        var toRemove = new List<Image>();

        foreach (var item in otherSet)
        {
            if (this.Contains(item))
            {
                toRemove.Add(item);
            }
            else
            {
                toAdd.Add(item);
            }
        }

        foreach (var item in toRemove)
        {
            this.Remove(item);
        }
        
        foreach (var item in toAdd)
        {
            this.Add(item);
        }
    }

    public void UnionWith(IEnumerable<Image> other)
    {
        if (other == null) throw new ArgumentNullException(nameof(other));
        foreach (var item in other)
        {
            Add(item);
        }
    }

    #endregion
}
