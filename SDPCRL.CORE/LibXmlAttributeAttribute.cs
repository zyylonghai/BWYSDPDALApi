using System;
using System.Collections.Generic;
using System.Text;

namespace SDPCRL.CORE
{
    public enum LibControlType
    {
        TextBox = 1,
        Combox = 2,
        TextAndBotton = 3


    }
    /// <summary>
    /// 控件名，控件类型，显示名称，是否只读，是否隐藏
    /// </summary>
    public class LibAttributeAttribute : Attribute
    {
        private string _controlNm;
        private LibControlType _controlType;
        private string _displayText;
        private bool _isReadOnly = false;
        private bool _hidden = false;
        /// <summary></summary>
        /// <param name="controlNm">控件名称</param>
        public LibAttributeAttribute(string controlNm)
            : base()
        {
            this._controlNm = controlNm;
        }
        /// <summary> </summary>
        /// <param name="controlNm">控件名称</param>
        /// <param name="controlType">控件类型</param>
        /// <param name="displayText">显示名称</param>
        public LibAttributeAttribute(string controlNm, LibControlType controlType, string displayText)
            : base()
        {
            this._controlNm = controlNm;
            this._controlType = controlType;
            this._displayText = displayText;
        }
        /// <summary></summary>
        /// <param name="controlNm">控件名称</param>
        /// <param name="controlType">控件类型</param>
        /// <param name="displayText">显示名称</param>
        /// <param name="isReadOnly">是否只读</param>
        public LibAttributeAttribute(string controlNm, LibControlType controlType, string displayText, bool isReadOnly)
            : base()
        {
            this._controlNm = controlNm;
            this._controlType = controlType;
            this._displayText = displayText;
            this._isReadOnly = isReadOnly;
        }
        /// <summary></summary>
        /// <param name="controlNm">控件名称</param>
        /// <param name="controlType">控件类型</param>
        /// <param name="displayText">显示名称</param>
        /// <param name="isReadOnly">是否只读</param>
        /// <param name="isHidden">是否隐藏</param>
        public LibAttributeAttribute(string controlNm, LibControlType controlType, string displayText, bool isReadOnly, bool isHidden)
            : base()
        {
            this._controlNm = controlNm;
            this._controlType = controlType;
            this._displayText = displayText;
            this._isReadOnly = isReadOnly;
            this._hidden = isHidden;
        }
        public string ControlNm
        {
            get { return _controlNm; }
        }
        public LibControlType ControlType
        {
            get { return this._controlType; }
        }

        public string DisplayText
        {
            get { return this._displayText; }
        }
        public bool IsReadOnly
        {
            get { return this._isReadOnly; }
        }
        public bool IsHidden
        { get { return this._hidden; } }
    }

    public class LibReSourceAttribute : Attribute
    {
        private string _reSource;
        public LibReSourceAttribute(string reSource)
            : base()
        {
            this._reSource = reSource;
        }
        public string Resource
        {
            get { return _reSource; }
        }
    }
}
