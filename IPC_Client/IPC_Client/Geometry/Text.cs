using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace INFOGET_ZERO_HULL.Geometry
{
    public class Text
    {
        public bool NeedToConvertTextToNote = false;

        public string CONTENT = string.Empty;
        public Point2D POSITION = new Point2D();
        public double HEIGHT = 0.1;
        public double ROTATION = 0.0;
        public double SLANT = 90;
        public double ASPECT = 1.0;
        public string COLOUR = "Black";
        public double TEXTLINEGAP = 0.0;
        public Point2D CandiStartPoint = new Point2D();
        public Point2D CandiEndPoint = new Point2D();
        public int LAYER = -1;
        public string LineType = "Solid";
        public string Font = "";

        public bool Underline = false;
        public bool Italic = false;

        //PDMS 방식.
        public int Alignment = 1;
        //PDMS 방식.
        public string EText = string.Empty;

        public string VIEWNAME = string.Empty;

        ////dmkim 170420
        ///// <summary>
        ///// SLOP/Dimension 노트 기존Comp핸들과 연결하기위해서.
        ///// </summary>
        //public Aveva.Marine.Drafting.MarElementHandle oCompViewHandle = new Aveva.Marine.Drafting.MarElementHandle();

        public Text()
        {}

        /// <summary>
        /// 생성자.
        /// </summary>
        /// <param name="content">내용</param>
        /// <param name="height">크기</param>
        /// <param name="pos">위치</param>
        /// <param name="font">폰트</param>
        /// <param name="rotation">회전</param>
        /// <param name="rotation">정렬</param>
        public Text(string content, double height,Point2D pos, string font, double rotation, string sColour = "Black")
        {
            this.CONTENT = content;
            this.HEIGHT = height;
            this.POSITION.SetFromPoint(pos);
            this.Font = font;
            this.ROTATION = rotation;

            this.COLOUR = sColour;
        }

        /// <summary>
        /// Align 정렬 (Alignment,POSITION) 값 변경.
        /// </summary>
        /// <param name="sAlign">정렬 LEFT,RIGHT,CENTER</param>
        /// <param name="dStartPosX">Start Pos X</param>
        /// <param name="dEndPosX">End Pos X</param>
        /// <param name="dTextGapX">X Gap</param>
        /// <param name="dY">Y</param>
        public void SetAlign(string sAlign, double dStartPosX, double dEndPosX, double dTextGapX, double dY)
        {
            if (sAlign == "LEFT")
            {
                //Default
            }
            else if (sAlign == "RIGHT")
            {
                this.Alignment = 3;
                this.POSITION.SetCoordinates(dEndPosX - dTextGapX, dY);
            }
            else if (sAlign == "CENTER")
            {
                this.Alignment = 2;
                Point2D oStart = new Point2D(dStartPosX + dTextGapX, dY);
                Point2D oEnd = new Point2D(dEndPosX, dY);
                this.POSITION.SetFromMidPoint(oStart, oEnd);
            }
        }


        public void Print()
        {
            Console.WriteLine("Content : {0}", this.CONTENT);
            Console.WriteLine("Pos X {0}, Y {1}", this.POSITION.X.ToString(), this.POSITION.Y.ToString());
            Console.WriteLine("Height : {0}", this.HEIGHT.ToString());
        }
    }
}
