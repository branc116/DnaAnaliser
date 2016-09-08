using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DnaAnaliser
{
    internal class GenomAnaliseCollection : IList<GenomAnalise>
    {
        private List<GenomAnalise> _list = new List<GenomAnalise>();

        public GenomAnaliseCollection(string FolderOfGeneFiles)
        {
            var filz = Directory.EnumerateFiles(FolderOfGeneFiles).Where(i => i.Contains(".23andme"));
            foreach (var fil in filz.AsParallel())
            {
                var NewAnal = new GenomAnalise(Path.Combine(FolderOfGeneFiles, fil));
                if (NewAnal.Count != 0)
                    Add(NewAnal);
            }
        }

        #region List

        public GenomAnalise this[int index]
        {
            get
            {
                return _list[index];
            }

            set
            {
                _list[index] = value;
            }
        }

        public int Count
        {
            get
            {
                return _list.Count;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public void Add(GenomAnalise item)
        {
            _list.Add(item);
        }

        public void Clear()
        {
            _list.Clear();
        }

        public bool Contains(GenomAnalise item)
        {
            return _list.Contains(item);
        }

        public void CopyTo(GenomAnalise[] array, int arrayIndex)
        {
            _list.CopyTo(array, arrayIndex);
        }

        public IEnumerator<GenomAnalise> GetEnumerator()
        {
            foreach (var gen in _list)
            {
                yield return gen;
            }
        }

        public int IndexOf(GenomAnalise item)
        {
            return _list.IndexOf(item);
        }

        public void Insert(int index, GenomAnalise item)
        {
            _list.Insert(index, item);
        }

        public bool Remove(GenomAnalise item)
        {
            return _list.Remove(item);
        }

        public void RemoveAt(int index)
        {
            _list.RemoveAt(index);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        #endregion List
    }
}