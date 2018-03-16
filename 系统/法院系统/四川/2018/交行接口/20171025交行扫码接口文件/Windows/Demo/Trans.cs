using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Demo
{

    [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct ByteParam
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 200)]
        public byte[] aucParam;
    }

    [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct CardDownData
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public byte[] aucRespCode;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 60)]
        public byte[] aucRespInfo;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
        public byte[] aucAmount;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public byte[] aucTransDate;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
        public byte[] aucTransTime;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public byte[] aucOrderNo;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
        public byte[] aucReserve;
    }

    class Trans
    {
        [DllImport("DLL_BANKCOMM_INTERFACE.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern int BANKCOMM_SetParam(int iParamIndex, string data);
        [DllImport("DLL_BANKCOMM_INTERFACE.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern int BANKCOMM_Login(ref CardDownData szOut, string deviceID = null);
        [DllImport("DLL_BANKCOMM_INTERFACE.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern int BANKCOMM_Purchase(int amount, string scancode, ref ByteParam detail, ref CardDownData szOut, string deviceID = null);
        [DllImport("DLL_BANKCOMM_INTERFACE.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern int BANKCOMM_Query(string orderNo, ref CardDownData szOut, string deviceID = null);
        [DllImport("DLL_BANKCOMM_INTERFACE.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern int BANKCOMM_Void(int amount, string orderNo, ref CardDownData szOut, string deviceID = null);
        [DllImport("DLL_BANKCOMM_INTERFACE.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern int BANKCOMM_Refund(int amount, string orderNo, ref CardDownData szOut, string deviceID = null);
        [DllImport("DLL_BANKCOMM_INTERFACE.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern int BANKCOMM_AutoPurchase(int amount, string scancode, ref ByteParam detail, int timeout, ref CardDownData szOut, string deviceID = null);

        public static void SetParam(int iParamIndex, String data)
        {
            BANKCOMM_SetParam(iParamIndex, data);
        }

        public static CardDownData Login()
        {
            CardDownData cardDownData = new CardDownData();
            BANKCOMM_Login(ref cardDownData);
            return cardDownData;
        }

        public static CardDownData Purchase(int amount, String channel, String detail, String authCode)
        {
            ByteParam byteDetail = new ByteParam();
            byte[] bsDetail = Encoding.UTF8.GetBytes(detail);
            byteDetail.aucParam = new byte[200];
            Array.Copy(bsDetail, byteDetail.aucParam, bsDetail.Length);
            CardDownData cardDownData = new CardDownData();
            BANKCOMM_Purchase(amount, channel+authCode, ref byteDetail, ref cardDownData);
            return cardDownData;
        }

        public static CardDownData Query(String orderNo)
        {
            CardDownData cardDownData = new CardDownData();
            BANKCOMM_Query(orderNo, ref cardDownData);
            return cardDownData;
        }

        public static CardDownData Void(int amount, String orderNo)
        {
            CardDownData cardDownData = new CardDownData();
            BANKCOMM_Void(amount, orderNo, ref cardDownData);
            return cardDownData;
        }

        public static CardDownData Refund(int amount, String orderNo)
        {
            CardDownData cardDownData = new CardDownData();
            BANKCOMM_Refund(amount, orderNo, ref cardDownData);
            return cardDownData;
        }

        public static CardDownData AutoPurchase(int amount, String channel, String detail, int timeout, String authCode, String deviceID)
        {
            ByteParam byteDetail = new ByteParam();
            byte[] bsDetail = Encoding.UTF8.GetBytes(detail);
            byteDetail.aucParam = new byte[200];
            Array.Copy(bsDetail, byteDetail.aucParam, bsDetail.Length);
            CardDownData cardDownData = new CardDownData();
            BANKCOMM_AutoPurchase(amount, channel+authCode, ref byteDetail, timeout, ref cardDownData, deviceID);
            return cardDownData;
        }
    }
}
