namespace AudioStreamerAPI.Constants
{
    public static class NumericConstants
    {
        //Random value I have no idea what is the minimum threshold
        //Just kidding, it's 0.6 - 1.xx
        //Yes I'm not joking, it expands beyond 1 on some cases but we can floor it down
        public const float MIN_RECOMMENDED_VALUE = 0.62f;
        public const int MAX_RECOMMENDED_ITEMS = 10;
        public const int MAX_TOP_TRACKS = 5;
    }
}
