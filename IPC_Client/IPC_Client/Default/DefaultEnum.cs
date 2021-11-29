using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;


namespace INFOGET_ZERO_HULL.Default
{
    /// <summary>
    /// 열거형으로 사용.
    /// using E = INFOGET.Default.DefaultEnum; 형태로 사용
    /// </summary>
    public static class DefaultEnum
    {
        /// <summary>
        /// 조선소 리스트
        /// </summary>
        public enum SHIPYARD
        {
            /// <summary>
            /// SHI 삼성중공업
            /// HHI 현대중공업
            /// HHISS 현대중공업 특수선사업부
            /// HMD 현대미포조선
            /// HSHI 현대삼호중공업
            /// STX STX 조선해양
            /// HJ 한진중공업
            /// SDSM 성동조선해양
            /// </summary>
            SHI,
            //HHI, 에러방지를 위해서.
            HHISS,
            HMD,
            HSHI,
            STX,
            HJ,
            SDSM
        }


        public enum TTY_VALUE
        {
            AUTOAP_BLOCKSITE,
            AUTOAP_REV1,
            AUTOAP_REV2,
            AUTOAP_REV3,
            AUTOAP_EMPTY,
            AUTOAP_ZONE
        }

        public enum SITE_BLOCK_ZONE
        {
            SITE,
            BLOCK,
            ZONE
        }

        public enum MemorySizeType
        {
            Byte, KByte, MByte, GByte, TByte
        }

        /// <summary>
        /// PIECE or ISO
        /// </summary>
        [DataContract]
        public enum DRAW_MODE
        {
            //dmkim 170927 PIECE-> SPOOL
            [EnumMember(Value = "SPOOL")]
            PIECE,
            [EnumMember(Value = "ISO")]
            ISO
        }

        #region VIEW관련

        /// <summary>
        /// 도면생성방식
        /// </summary>
        public enum DWGNEW_METHOD
        {
            /// <summary>
            /// PDMS : NEW DRWG, NEW SHEE...
            /// MARAPI : MARDRAFTING.DWGNEW...
            /// </summary>
            PDMS,
            MARAPI
        }

        /// <summary>
        /// 도면그리는 방식
        /// </summary>
        public enum DWGDRAW_METHOD
        {
            /// <summary>
            /// PDMS : NEW STRA
            /// MARAPI : DRAFT.LINENEW()
            /// </summary>
            PDMS,
            MARAPI
        }

        /// <summary>
        /// VIEW DIRECTION (ISO1234,PLAN,SEC,ELEV)
        /// </summary>
        public enum VIEW_DIR
        {
            /// <summary>
            /// ISO,PLAN,ELEV,SEC
            /// USER : Direction 별도 사용.
            /// </summary>
            ISO1,
            ISO2,
            ISO3, //ISO기본.
            ISO4,
            PLAN,
            ELEV,
            SEC,
            USER, //정규방향이 아닌.
            NOTSET
        }

        /// <summary>
        /// Enum은 아니지만.. VIEW의 VTYPE
        /// </summary>
        public class VTYPE
        {
            /// <summary>
            /// Wireline
            /// Wireline Hidden Line
            /// Modelled Wireline
            /// Local Hidden Line
            /// Global Hidden Line
            /// Universal Hidden Line
            ///
            /// </summary>
            public static string Wireline = "Wireline";
            public static string WireHLine = "WireHLine";
            public static string MWireline = "MWireline";
            public static string LocalHLine = "Local HLine";
            public static string GlobalHLine = "Global HLine";
            public static string UniversalHLine = "Universal HLine";
        }

        /// <summary>
        /// VIEW 타입 (ISOGEN)
        /// </summary>
        public enum VIEW_TYPE
        {
            /// <summary>
            /// A3,A4 PIECE TYPE, ISO TYPE
            /// MULTI 멀티관련
            /// BRACKET 브라켓관련
            /// COAMING 코밍관련
            /// MATERIAL 자재정보관련
            /// MANUFACTURING 생산정보관련
            /// NOTSET 설정안됨.
            /// </summary>
            A3_PIECE_ISOGEN,
            A3_PIECE_ISO,
            A3_PIECE_SEC,
            A3_PIECE_ELEV,
            A3_PIECE_PLAN,
            A3_PIECE_ISO_RB,
            A3_PIECE_ISO_LB,

            //A4_PIECE_ISOGEN,
            //A4_PIECE_ISO,
            //A4_PIECE_SEC,
            //A4_PIECE_ELEV,
            //A4_PIECE_PLAN,

            A3_ISO_ISOGEN,
            A3_ISO_ISO,
            A3_ISO_SEC,
            A3_ISO_ELEV,
            A3_ISO_PLAN,

            //A4_ISO_ISOGEN,
            //A4_ISO_ISO,
            //A4_ISO_SEC,
            //A4_ISO_ELEV,
            //A4_ISO_PLAN,

            //A3_PIECE_MULTI,
            A3_PIECE_BRACKET_ISOGEN,
            A3_PIECE_BRACKET1,
            A3_PIECE_BRACKET2,
            A3_PIECE_BRACKET3,
            A3_PIECE_BRACKET4,
            A3_PIECE_BRACKET5,
            A3_PIECE_BRACKET6,
            A3_PIECE_BRACKET_PLAN,
            //A3_PIECE_COAMING,
            A3_COAMING_PLAN,
            A3_COAMING_SEC,
            A3_COAMING_ISO3,

            A3_PIECE_MULTIPENE_ISOGEN,
            A3_PIECE_MULTIPENE_PLAN,

            A3_PIECE_MULTIPENE_ISO,
            A3_PIECE_MULTIPENE_PLAN1,
            A3_PIECE_MULTIPENE_ELEV,
            A3_PIECE_MULTIPENE_SEC,

            A3_PIECE_SINGLEPENE_PLAN,

            //A3_PIECE_MATERIAL,
            //A3_PIECE_MANUFACTURING,

            TOUCH_UP,

            NOTSET
        }

        /// <summary>
        /// FORM 넣는 방식 FORM / GEN_SUBPICTURE / STD_SUBPICTURE / BSRF
        /// </summary>
        public enum FORM_INSERT_METHOD
        {
            FORM,
            GEN_SUBPICTURE,
            STD_SUBPICTURE,
            BSRF
        }

        /// <summary>
        /// DRAFT DB ELEMENT TYPE
        /// </summary>
        public enum DRAFT_ELEMENT_TYPE
        {
            DEPT,
            REGI,
            DRWG,
            SHEE,
            LIBY,
            DLLB,
            IDLI,
            ADDE,
            VIEW,
            LAYE
        }
        #endregion


        /// <summary>
        /// 현재프로젝트
        /// </summary>
        public enum PROJECT
        {
            NUMBER,
            CODE,
            DESC,
            VERSION,
            MODULE,
            ID
        }


        public enum MODULE
        {
            Monitor,
            Design,
            HullDesign,
            MarineDrafting,
            Outfitting,
            OutfittingDraft,
            Diagrams,
            Spooler,
            Draft,
            IsoDraft,
            Paragon,
            Specon,
            Propcon,
            Lexicon,
            Admin,
            Tags
        }

        [DataContract]
        public enum STYLE
        {
            [EnumMember(Value = "REGULAR")]
            REGULAR,
            [EnumMember(Value = "BOLD")]
            BOLD,
            [EnumMember(Value = "ITALIC")]
            ITALIC,
            [EnumMember(Value = "BOLD_ITALIC")]
            BOLD_ITALIC,
            [EnumMember(Value = "UNDERLINE")]
            UNDERLINE,
            [EnumMember(Value = "UNDERLINE_ITALIC")]
            UNDERLINE_ITALIC,
            [EnumMember(Value = "STRIKEOUT")]
            STRIKEOUT,
            [EnumMember(Value = "STRIKEOUT_ITALIC")]
            STRIKEOUT_ITALIC
        }

        [DataContract]
        public enum ALIGNMENT
        {
            [EnumMember(Value = "LEFT")]
            LEFT,
            [EnumMember(Value = "RIGHT")]
            RIGHT,
            [EnumMember(Value = "CENTER")]
            CENTER
        }

        public enum DIRECTION
        {
            LEFT,
            RIGHT,
            TOP,
            DOWN
        }

        [DataContract]
        public enum COLOR
        {
            [EnumMember(Value = "GREY")]
            GREY,
            [EnumMember(Value = "RED")]
            RED,
            [EnumMember(Value = "ORANGE")]
            ORANGE,
            [EnumMember(Value = "YELLOW")]
            YELLOW,
            [EnumMember(Value = "GREEN")]
            GREEN,
            [EnumMember(Value = "CYAN")]
            CYAN,
            [EnumMember(Value = "BLUE")]
            BLUE,
            [EnumMember(Value = "VIOLET")]
            VIOLET,
            [EnumMember(Value = "BROWN")]
            BROWN,
            [EnumMember(Value = "WHITE")]
            WHITE,
            [EnumMember(Value = "PINK")]
            PINK,
            [EnumMember(Value = "MAUVE")]
            MAUVE,
            [EnumMember(Value = "TURQUOISE")]
            TURQUOISE,
            [EnumMember(Value = "INDIGO")]
            INDIGO,
            [EnumMember(Value = "BLACK")]
            BLACK,
            [EnumMember(Value = "MAGENTA")]
            MAGENTA
        }



        //dmkim 190717
        public enum COORDINATE
        {
            XYZ,
            ENU
        }
    }
}
