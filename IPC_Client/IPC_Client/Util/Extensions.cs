using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace INFOGET_ZERO_HULL
{
    public static class Extensions
    {

        /// <summary>
        /// [확장] String을 Boolean으로, 실패할경우 False
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool ToBoolean(this string s)
        {
            bool result;
            if (!bool.TryParse(s, out result))
            {
                result = false;
            }
            return result;
        }
        /// <summary>
        /// [확장] String Null/""이 아닌지 체크
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public static bool IsNotNull(this string param)
        {
            return (param != null && param != "") ? true : false;
        }
        /// <summary>
        /// [확장] String Null/"" 인지 체크
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public static bool IsNull(this string param)
        {
            return (param == null || param == "") ? true : false;
        }

        /// <summary>
        /// [확장] String cSplitter로 Split하여 List<string>로 리턴.
        /// </summary>
        /// <param name="sString"></param>
        /// <param name="cSplitter"></param>
        /// <returns></returns>
        public static List<string> stringSplit(this string sString, char cSplitter)
        {
            List<string> olRtn = new List<string> { };

            string[] olSplit = sString.Split(cSplitter);
            foreach (string sSplit in olSplit)
            {
                olRtn.Add(sSplit.Trim());
            }

            return olRtn;
        }

        /// <summary>
        /// [확장] Key값이 없을경우만 ADD
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dictionary"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void AddOnly<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, TValue value)
        {
            if (!dictionary.ContainsKey(key))
            {
                dictionary.Add(key, value);
            }
        }

        /// <summary>
        /// [확장] RichTextBox에 Color 적용.
        /// </summary>
        /// <param name="box"></param>
        /// <param name="text"></param>
        /// <param name="color"></param>
        public static void AppendText(this RichTextBox box, string text, Color color)
        {
            box.SelectionStart = box.TextLength;
            box.SelectionLength = 0;

            box.SelectionColor = color;
            box.AppendText(text);
            box.SelectionColor = box.ForeColor;
        }

        /// <summary>
        /// [확장] string[]을 List<string>로 변환
        /// </summary>
        /// <param name="strArray"></param>
        /// <returns></returns>
        private static List<string> ToList(this string[] strArray)
        {
            List<string> olRtn = new List<string> { };
            foreach (string sStr in strArray) { olRtn.Add(sStr); }
            return olRtn;
        }

        /// <summary>
        /// [확장] Dictionary Key가 없으면 Value를 추가하고 있으면 Update한다.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Dic"></param>
        /// <param name="Key"></param>
        /// <param name="Value"></param>
        public static void AddOrModify<T>(this Dictionary<string, T> Dic, string Key, T Value)
        {
            if (Dic.ContainsKey(Key))
                Dic[Key] = Value;
            else
                Dic.Add(Key, Value);
        }

    }
}
