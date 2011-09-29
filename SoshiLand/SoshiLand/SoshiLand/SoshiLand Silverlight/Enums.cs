using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoshiLandSilverlight
{
    public enum Props
    {
        None,
        LaScala,
        Bali,
        TempleMount,
        DamnoenMarket,
        GreatWall,
        TajMahal,
        StatueLiberty,
        EiffelTower,
        Parthenon,
        WhiteHouse,
        GyeongBokGoong,
        MountEverest,
        GrandCanal,
        VenetianResort,
        ChateauDeChillon,
        TokyoDome,
        Colosseum,
        BlueHouse,
        BukitTimah,
        CNTower,
        KuwaitMuseum,
        WalkOfFame,
        AngkorWat,
        Disneyland,
        AmazonRainforest,
        HongKong,
        UNHQ,
        SydneyOpera,
        GoldenGateBridge,
        MalibuBeach,
        BarcelonaAirport,
        WencelsasSquare,
        BarrierReef,
        Pisa,
        BigBen,
        GizaPyramid,
        Chance1,
        Forever9,
        CommChest1,
        CommChest2,
        ShoppingSpree,
        LuxuryTax,
        Chance2,
        SoshiBond,
        Go,
        Babysit,
        GoBabysit,
        FanMeeting
    }

    public enum TileType
    {
        Go,
        Property,
        Chance,
        CommunityChest,
        Jail,
        SpecialLuxuryTax,
        ShoppingSpree,
        Utility,
        FanMeeting,
        GoToJail
    }

    public enum SpecialCardType
    {
        None,
        GetOutOfJailFreeCard,
        GoToJailCard,
        CanPassGo
    }

    public enum GameState
    {
        EnterUserName,
        ChatRoom,
        InGame
    }

    public enum BoardPiece
    {
        ITNW_Taeyeon,
        ITNW_Jessica,
        ITNW_Sunny,
        ITNW_Tiffany,
        ITNW_Hyoyeon,
        ITNW_Yuri,
        ITNW_Sooyoung,
        ITNW_Yoona,
        ITNW_Seohyun
    }

    public enum RowLocation
    {
        TopRow,
        BottomRow,
        LeftColumn,
        RightColumn
    }
}
