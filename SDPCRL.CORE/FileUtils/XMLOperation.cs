using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace SDPCRL.CORE.FileUtils
{
    /// <summary>xml文档操作类</summary>
    public class XMLOperation
    {
        #region 私有属性
        private string _xmlFilePath = string.Empty;
        private XmlDocument _doc = null;
        //private XmlNodeList _currentNodeList = null;
        //priv 
        //private int _nodeIndex = 0;
        #endregion
        #region 共有属性
        /// <summary>xml文档路径</summary>
        public string XMLFilePath
        {
            get { return _xmlFilePath; }
            set { _xmlFilePath = value; }
        }
        /// <summary>根节点名称</summary>
        public string RootNodeName
        {
            get;
            set;
        }
        #endregion

        #region 构造函数
        public XMLOperation(string xmlFilePath)
        {
            _xmlFilePath = xmlFilePath;
            CreateXMLDoc();
        }
        #endregion
        #region 私有函数
        /// <summary>
        /// 
        /// </summary>
        private void CreateXMLDoc()
        {
            try
            {
                if (_doc == null)
                    _doc = new XmlDocument();
                _doc.Load(_xmlFilePath);
            }
            catch (Exception ex)
            {
                _doc = null;
            }
        }
        #endregion

        #region 公开函数
        public ILibXMLNodeRead NodeRead(string nodeExpress)
        {
            return new LibXMLNodeRead(this._doc, nodeExpress);
        }

        #endregion

        #region xml节点增加，删除，修改。
        public bool AddNode(NodeInfo node, string express)
        {
            XmlNode parentNode = SelectNode(express);
            XmlElement child = _doc.CreateElement(node.NodeName);
            if (string.Compare(node.NodeName, SysConstManage.ClassNodeNm) != 0)
                child.InnerText = node.InnerText;
            if (node.Attributions.DefindAttr != null)
            {
                foreach (KeyValuePair<string, object> keyvalu in node.Attributions.DefindAttr)
                {
                    child.SetAttribute(keyvalu.Key, keyvalu.Value.ToString());
                }
            }
            parentNode.AppendChild(child);
            this._doc.Save(this._xmlFilePath);
            return true;
        }

        public bool UpdateNode(NodeInfo node, string express)
        {
            XmlNode currentNode = SelectNode(express);
            if (string.Compare(node.NodeName, SysConstManage.ClassNodeNm) != 0)
                currentNode.InnerText = node.InnerText;
            if (node.Attributions.DefindAttr != null)
            {
                XmlElement ele = (XmlElement)currentNode;
                foreach (KeyValuePair<string, object> keyvalu in node.Attributions.DefindAttr)
                {
                    ele.SetAttribute(keyvalu.Key, keyvalu.Value.ToString());
                }
            }
            this._doc.Save(this._xmlFilePath);
            return true;

        }

        public bool DeletNode(string express)
        {
            XmlNode currentNode = SelectNode(express);
            currentNode.ParentNode.RemoveChild(currentNode);
            this._doc.Save(this._xmlFilePath);
            return true;
        }

        private XmlNode SelectNode(string express)
        {
            XmlNode result = null;
            try
            {
                result = this._doc.SelectSingleNode(express);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            if (result == null) throw new LibExceptionBase("查找ModelTreeTemp文件的节点时出错：找不到节点");
            return result;
        }
        #endregion

        /// <summary>xml节点读取类</summary>
        class LibXMLNodeRead : ILibXMLNodeRead
        {
            #region 私有变量
            XmlDocument _doc;
            XmlNodeList _nodeList;
            XmlNode _currentNode;
            string _nodeExpress = string.Empty;
            private int _nodeIndex = 0;
            #endregion

            #region 构造函数
            public LibXMLNodeRead(XmlDocument doc, string nodeExpress)
            {
                this._doc = doc;
                this._nodeExpress = nodeExpress;
                DoFindNode();
            }
            #endregion

            #region 私有方法
            private void DoFindNode()
            {
                if (_doc != null && _nodeList == null)
                    _nodeList = _doc.SelectNodes(_nodeExpress);
                if (_nodeList != null && _nodeList.Count > 0)
                    _currentNode = _nodeList[_nodeIndex];


            }
            #endregion

            #region ILibXMLNodeRead 接口实现

            public void ReadNext()
            {
                _nodeIndex++;
                DoFindNode();
            }

            public bool EOF
            {
                get
                {
                    return !(_nodeList != null && _nodeIndex < _nodeList.Count);
                }
            }

            public string CurrentNodeName
            {
                get
                {
                    return _currentNode == null ? string.Empty : _currentNode.Name;
                }
            }


            public LibXMLAttributCollection Attributions
            {
                get
                {
                    if (_currentNode != null)
                        return new LibXMLAttributCollection(_currentNode.Attributes);
                    else
                        return null;
                }
            }

            public string OuterXML
            {
                get { return _currentNode == null ? string.Empty : _currentNode.OuterXml; }
            }

            public string InnerText
            {
                get { return _currentNode == null ? string.Empty : _currentNode.InnerText; }
            }

            public bool HasChildNode
            {
                get { return _currentNode == null ? false : _currentNode.HasChildNodes; }
            }

            public XmlNode CurrentNode
            {
                get { return _currentNode; }
            }

            public NodeInfo ReadChild(int index)
            {
                NodeInfo info = null;

                XmlNode node = CurrentNode.ChildNodes[index];
                if (node != null)
                {
                    info = new NodeInfo(node);

                }
                return info;
            }
            #endregion
        }
    }
    /// <summary>xml节点属性集 </summary>
    public class LibXMLAttributCollection
    {
        XmlAttributeCollection _attrCollection = null;
        Dictionary<string, object> _defindAttr;
        public string this[string name]
        {
            get { return _attrCollection[name].Value; }
        }

        public Dictionary<string, object> DefindAttr
        {
            get { return this._defindAttr; }
        }
        public void Add(string attrNm, object attrValu)
        {
            if (_defindAttr == null) _defindAttr = new Dictionary<string, object>();
            _defindAttr.Add(attrNm, attrValu);

        }
        #region 构造函数
        public LibXMLAttributCollection(XmlAttributeCollection attrCollection)
        {
            this._attrCollection = attrCollection;
        }
        public LibXMLAttributCollection()
        { }
        #endregion
    }
    /// <summary>xml节点读取接口</summary>
    public interface ILibXMLNodeRead
    {
        /// <summary>当前查找节点集 游标是否指到最后节点 </summary>
        bool EOF
        {
            get;
        }
        /// <summary>当前节点名称</summary>
        string CurrentNodeName
        {
            get;
        }
        /// <summary>节点属性集</summary>
        LibXMLAttributCollection Attributions
        {
            get;
        }
        /// <summary> 获取该节点及其所有子节点的xml标记</summary>
        string OuterXML
        {
            get;
        }
        /// <summary>获取该节点及其 所有子节点的串联值</summary>
        string InnerText
        {
            get;
        }
        /// <summary>是否有子节点 </summary>
        bool HasChildNode
        {
            get;
        }
        /// <summary>
        /// 当前节点
        /// </summary>
        XmlNode CurrentNode { get; }
        NodeInfo ReadChild(int index);

        void ReadNext();
    }


    public class NodeInfo
    {
        XmlNode _node;
        LibXMLAttributCollection _attributcollection;
        string _nodeNm;
        string _innerText;
        public NodeInfo()
        { }
        public NodeInfo(XmlNode node)
        {
            this._node = node;
        }
        /// <summary>当前节点名称</summary>
        public string NodeName
        {
            get
            {
                if (_node != null)
                    return _node.Name;
                return _nodeNm;
            }
            set { _nodeNm = value; }
        }
        /// <summary>获取该节点及其 所有子节点的串联值</summary>
        public string InnerText
        {
            get
            {
                if (this._node != null)
                    return _node.InnerText;
                return _innerText;
            }
            set { _innerText = value; }
        }
        /// <summary>节点属性集</summary>
        public LibXMLAttributCollection Attributions
        {
            get
            {
                if (_node != null)
                    return new LibXMLAttributCollection(_node.Attributes);
                return _attributcollection;
            }
            set { _attributcollection = value; }
        }
    }
}
