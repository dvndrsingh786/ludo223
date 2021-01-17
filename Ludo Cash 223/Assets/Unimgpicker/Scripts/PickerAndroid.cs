#if UNITY_ANDROID
using UnityEngine;

namespace Kakera
{
    internal class PickerAndroid : IPicker
    {
        private static readonly string PickerClass = "com.kakeragames.unimgpicker.Picker";

        public void Show(string title, string outputFileName)
        {
            using (var picker = new AndroidJavaClass(PickerClass))
            {
                picker.CallStatic("show", title, outputFileName);
            }
        }
        public void Show1(string title, string outputFileName)
        {
            using (var picker = new AndroidJavaClass(PickerClass))
            {
                picker.CallStatic("show1", title, outputFileName);
            }
        }
    }
}
#endif