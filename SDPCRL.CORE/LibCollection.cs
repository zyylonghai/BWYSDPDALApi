using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Xml.Serialization;

namespace SDPCRL.CORE
{
    public class LibCollection<T> : ICollection
    {
        #region 私有属性
        private ArrayList _entityArray;
        #endregion

        #region 公开属性
        public T this[int index]
        {
            get { return (T)_entityArray[index]; }
        }
        [XmlAttribute]
        public string Guid { get; set; }

        #endregion
        public void Add(T item)
        {
            if (_entityArray == null)
                _entityArray = new ArrayList();
            _entityArray.Add(item);
        }
        public void CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }
        /// <summary>根据属性名，值 查找所有符合条件的项</summary>
        /// <param name="propertyNm">属性名</param>
        /// <param name="value">属性的值</param>
        /// <returns></returns>
        public T[] Find(string propertyNm, object value)
        {
            return DoFind(propertyNm, value);
        }
        /// <summary>根据属性名，值 查找所有符合条件的项，返回第一项</summary>
        /// <param name="propertyNm">属性名</param>
        /// <param name="value">属性的值</param>
        /// <returns></returns>
        public T FindFirst(string propertyNm, object value)
        {
            T[] array = DoFind(propertyNm, value);
            if (array.Length == 0)
            {
                return default(T);
            }
            return array[0];
        }

        public void Remove(string propertyNm, object value)
        {
            T obj = default(T);
            Type tp = typeof(T);
            PropertyInfo p = tp.GetProperty(propertyNm);
            if (p == null)
            {
                throw new LibExceptionBase(string.Format("属性{0}不存在", propertyNm));
            }
            foreach (T item in _entityArray)
            {
                object val = p.GetValue(item, null);
                if (value.Equals(val))
                {
                    obj = item;
                    break;
                }
            }
            if (obj != null)
            {
                _entityArray.Remove(obj);

            }

        }

        public void Remove(T obj)
        {
            _entityArray.Remove(obj);
        }

        public int Count
        {
            get
            {
                if (_entityArray == null)
                    _entityArray = new ArrayList();
                return _entityArray.Count;
            }
        }

        public bool IsSynchronized
        {
            get { throw new NotImplementedException(); }
        }

        public object SyncRoot
        {
            get { throw new NotImplementedException(); }
        }

        public IEnumerator GetEnumerator()
        {
            if (_entityArray == null)
                _entityArray = new ArrayList();
            return _entityArray.GetEnumerator();
        }

        #region 私有函数
        private T[] DoFind(string propertyNm, object value)
        {
            T[] result = { };
            Type tp = typeof(T);
            PropertyInfo p = tp.GetProperty(propertyNm);
            if (p == null)
            {
                throw new LibExceptionBase(string.Format("属性{0}不存在", propertyNm));
            }
            if (_entityArray == null) return result;
            foreach (T item in _entityArray)
            {
                object val = p.GetValue(item, null);
                if (value.Equals(val))
                {
                    Array.Resize(ref result, result.Length + 1);
                    result[result.Length - 1] = item;
                }
            }
            return result;
        }
        #endregion
    }
}
