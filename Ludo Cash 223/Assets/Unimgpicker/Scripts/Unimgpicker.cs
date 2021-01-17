using Kakera;
using System;
using UnityEngine;


    public class Unimgpicker : MonoBehaviour
    {
        public delegate void ImageDelegate(string path);

        public delegate void ErrorDelegate(string message);

        public event ImageDelegate Completed;
        public event ImageDelegate Completed1;
        public event ImageDelegate Completed2;
        public event ErrorDelegate Failed;

        private IPicker picker =

#if UNITY_EDITOR
            new Picker_editor();
#elif UNITY_IOS
            new PickeriOS();
#elif UNITY_ANDROID
            new PickerAndroid();
#else
            new PickerUnsupported();
#endif

        [Obsolete("Resizing is deprecated. Use Show(title, outputFileName)")]
        public void Show(string title, string outputFileName, int maxSize)
        {
            Show(title, outputFileName);
           Show1(title, outputFileName);
        }

        public void Show(string title, string outputFileName)
        {
            picker.Show(title, outputFileName);
        }

        public void Show1(string title, string outputFileName)
        {
            picker.Show1(title, outputFileName);
        }


    private void OnComplete(string path)
        {
            var handler = Completed;
            if (handler != null)
            {
                handler(path);
            }
        }
        private void OnComplete1(string path)
        {
            var handler = Completed1;
            if (handler != null)
            {
                handler(path);
            }
        }
        private void OnComplete2(string path)
        {
            var handler = Completed2;
            if (handler != null)
            {
                handler(path);
            }
        }

        private void OnFailure(string message)
        {
            var handler = Failed;
            if (handler != null)
            {
                handler(message);
            }
        }
    }