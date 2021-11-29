using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace INFOGET_ZERO_HULL.Geometry
{
    /// <summary>
    /// Marine Drafting에서 사용할 colour
    /// 12.1.SP3.0[9060]
    /// </summary>
    public abstract class Colour
    {
        
        public static readonly string WHITE            = "White";
        public static readonly string CYAN             = "Cyan";
        public static readonly string BLUE             = "Blue";
        public static readonly string MAGENTA          = "Magenta";
        public static readonly string RED              = "Red";
        public static readonly string YELLOW           = "Yellow";
        public static readonly string GREEN            = "Green";
        public static readonly string BLACK            = "Black";
        public static readonly string WHEAT            = "Wheat";
        public static readonly string MEDIUMAQUAMARINE = "MediumAquamarine";
        public static readonly string NAVYBLUE         = "NavyBlue";
        public static readonly string DARKORCHID       = "DarkOrchid";
        public static readonly string FIREBRICK        = "FireBrick";
        public static readonly string ORANGE           = "Orange";
        public static readonly string FORESTGREEN      = "ForestGreen";
        public static readonly string DIMGREY          = "DimGrey";
        public static readonly string TAN              = "Tan";
        public static readonly string AQUAMARINE       = "Aquamarine";
        public static readonly string SLATEBLUE        = "SlateBlue";
        public static readonly string VIOLET           = "Violet";
        public static readonly string INDIANRED        = "IndianRed";
        public static readonly string GOLD             = "Gold";
        public static readonly string LIMEGREEN        = "LimeGreen";
        public static readonly string GREY             = "Grey";
        public static readonly string SIENNA           = "Sienna";
        public static readonly string TURQUOISE        = "Turquoise";
        public static readonly string LIGHTBLUE        = "LightBlue";
        public static readonly string BLUEVIOLET       = "BlueViolet";
        public static readonly string PINK             = "Pink";
        public static readonly string CORAL            = "Coral";
        public static readonly string SPRINGGREEN      = "SpringGreen";
        public static readonly string LIGHTGREY        = "LightGrey";
        public static readonly string GREYT50          = "GreyT50";
        public static readonly string MAROON           = "Maroon";
        public static readonly string ORANGRED         = "OrangRed";
        public static readonly string CORALRED         = "CoralRed";
        public static readonly string TOMATO           = "Tomato";
        public static readonly string CHOCOLATE        = "Chocolate";
        public static readonly string SANDYBROWN       = "SandyBrown";
        public static readonly string DARKBROWN        = "DarkBrown";
        public static readonly string LIGHTGOLD        = "LightGold";
        public static readonly string BEIGE            = "Beige";
        public static readonly string BRIGHTORANGE     = "BrightOrange";
        public static readonly string LIGHTYELLOW      = "LightYellow";
        public static readonly string KAHKI            = "Kahki";
        public static readonly string YELLOWGREEN      = "YellowGreen";
        public static readonly string DARKGREEN        = "DarkGreen";
        public static readonly string WHITESMOKE       = "WhiteSmoke";
        public static readonly string DARKSLATEGREY    = "DarkSlateGrey";
        public static readonly string POWDERBULE       = "PowderBule";
        public static readonly string STEELBLUE        = "SteelBlue";
        public static readonly string ROYALBLUE        = "RoyalBlue";
        public static readonly string MIDNIGHTBLUE     = "MidnightBlue";
        public static readonly string PLUM             = "Plum";
        public static readonly string INDIGO           = "Indigo";
        public static readonly string MAUVE            = "Mauve";
        public static readonly string DEEPPINK         = "DeepPink";
        public static readonly string SALMON           = "Salmon";
        public static readonly string BROWN            = "Brown";
        public static readonly string DARKGREY         = "DarkGrey";
        public static readonly string IVORY            = "Ivory";
        public static readonly string BRIGHTRED        = "BrightRed";

        public static int GetNoOfColour(string sColour)
        {
            int iRtn = 1;

            if (sColour == Colour.GREY || sColour == "GREY") { iRtn = 1; }
            else if (sColour == Colour.RED || sColour == "RED") { iRtn = 2; }
            else if (sColour == Colour.ORANGE || sColour == "ORANGE") { iRtn = 3; }
            else if (sColour == Colour.YELLOW || sColour == "YELLOW") { iRtn = 4; }
            else if (sColour == Colour.GREEN || sColour == "GREEN") { iRtn = 5; }
            else if (sColour == Colour.CYAN || sColour == "CYAN") { iRtn = 6; }
            else if (sColour == Colour.BLUE || sColour == "BLUE") { iRtn = 7; }
            else if (sColour == Colour.VIOLET || sColour == "VIOLET") { iRtn = 8; }
            else if (sColour == Colour.BROWN || sColour == "BROWN") { iRtn = 9; }
            else if (sColour == Colour.WHITE || sColour == "WHITE") { iRtn = 10; }

            else if (sColour == Colour.PINK || sColour == "PINK") { iRtn = 11; }
            else if (sColour == Colour.MAUVE || sColour == "MAUVE") { iRtn = 12; }
            else if (sColour == Colour.TURQUOISE || sColour == "TURQUOISE") { iRtn = 13; }
            else if (sColour == Colour.INDIGO || sColour == "INDIGO") { iRtn = 14; }
            else if (sColour == Colour.BLACK || sColour == "BLACK") { iRtn = 15; }
            else if (sColour == Colour.MAGENTA || sColour == "MAGENTA") { iRtn = 16; }

            return iRtn;
        }

        #region 모양유지
        //public static readonly string WHITE            = "White";
        //public static readonly string CYAN             = "Cyan";
        //public static readonly string BLUE             = "Blue";
        //public static readonly string MAGENTA          = "Magenta";
        //public static readonly string RED              = "Red";
        //public static readonly string YELLOW           = "Yellow";
        //public static readonly string GREEN            = "Green";
        //public static readonly string BLACK            = "Black";
        //public static readonly string WHEAT            = "Wheat";
        //public static readonly string MEDIUMAQUAMARINE = "MediumAquamarine";
        //public static readonly string NAVYBLUE         = "NavyBlue";
        //public static readonly string DARKORCHID       = "DarkOrchid";
        //public static readonly string FIREBRICK        = "FireBrick";
        //public static readonly string ORANGE           = "Orange";
        //public static readonly string FORESTGREEN      = "ForestGreen";
        //public static readonly string DIMGREY          = "DimGrey";
        //public static readonly string TAN              = "Tan";
        //public static readonly string AQUAMARINE       = "Aquamarine";
        //public static readonly string SLATEBLUE        = "SlateBlue";
        //public static readonly string VIOLET           = "Violet";
        //public static readonly string INDIANRED        = "IndianRed";
        //public static readonly string GOLD             = "Gold";
        //public static readonly string LIMEGREEN        = "LimeGreen";
        //public static readonly string GREY             = "Grey";
        //public static readonly string SIENNA           = "Sienna";
        //public static readonly string TURQUOISE        = "Turquoise";
        //public static readonly string LIGHTBLUE        = "LightBlue";
        //public static readonly string BLUEVIOLET       = "BlueViolet";
        //public static readonly string PINK             = "Pink";
        //public static readonly string CORAL            = "Coral";
        //public static readonly string SPRINGGREEN      = "SpringGreen";
        //public static readonly string LIGHTGREY        = "LightGrey";
        //public static readonly string GREYT50          = "GreyT50";
        //public static readonly string MAROON           = "Maroon";
        //public static readonly string ORANGRED         = "OrangRed";
        //public static readonly string CORALRED         = "CoralRed";
        //public static readonly string TOMATO           = "Tomato";
        //public static readonly string CHOCOLATE        = "Chocolate";
        //public static readonly string SANDYBROWN       = "SandyBrown";
        //public static readonly string DARKBROWN        = "DarkBrown";
        //public static readonly string LIGHTGOLD        = "LightGold";
        //public static readonly string BEIGE            = "Beige";
        //public static readonly string BRIGHTORANGE     = "BrightOrange";
        //public static readonly string LIGHTYELLOW      = "LightYellow";
        //public static readonly string KAHKI            = "Kahki";
        //public static readonly string YELLOWGREEN      = "YellowGreen";
        //public static readonly string DARKGREEN        = "DarkGreen";
        //public static readonly string WHITESMOKE       = "WhiteSmoke";
        //public static readonly string DARKSLATEGREY    = "DarkSlateGrey";
        //public static readonly string POWDERBULE       = "PowderBule";
        //public static readonly string STEELBLUE        = "SteelBlue";
        //public static readonly string ROYALBLUE        = "RoyalBlue";
        //public static readonly string MIDNIGHTBLUE     = "MidnightBlue";
        //public static readonly string PLUM             = "Plum";
        //public static readonly string INDIGO           = "Indigo";
        //public static readonly string MAUVE            = "Mauve";
        //public static readonly string DEEPPINK         = "DeepPink";
        //public static readonly string SALMON           = "Salmon";
        //public static readonly string BROWN            = "Brown";
        //public static readonly string DARKGREY         = "DarkGrey";
        //public static readonly string IVORY            = "Ivory";
        //public static readonly string BRIGHTRED        = "BrightRed"; 
        #endregion

    }
}
