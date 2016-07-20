using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Database;
using Java.Lang;
using System.Linq;
using System.Collections.Generic;
using SimplexMethod;

namespace SimplexMethodAndroid
{
    [Activity(Label = "SimplexMethodAndroid", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        MatrixViewController matrixViewController;
        int count = 1;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Get our button from the layout resource,
            // and attach an event to it
            Button button = FindViewById<Button>(Resource.Id.MyButton);

            button.Click += Calculate;

            matrixViewController = new MatrixViewController(FindViewById<TextView>(Resource.Id.Text_MatrixOutput));

            FindViewById<Button>(Resource.Id.Button_Next).Click += matrixViewController.ButtonClick_Next;
            FindViewById<Button>(Resource.Id.Button_Prevous).Click += matrixViewController.ButtonClick_Previous;

            CheckBox cb = FindViewById<CheckBox>(Resource.Id.Toggle_FullRoots);
            cb.Checked = OutputFormat.fullRoots;
            cb.CheckedChange += delegate { OutputFormat.fullRoots = cb.Checked; };

            CheckBox cb2 = FindViewById<CheckBox>(Resource.Id.Toggle_RawOutput);
            cb2.Checked = OutputFormat.rawFormat;
            cb2.CheckedChange += delegate { OutputFormat.rawFormat = cb2.Checked; };

            EditText et2 = FindViewById<EditText>(Resource.Id.editText2);
            et2.AfterTextChanged += delegate { int r; if (int.TryParse(et2.Text, out r)) OutputFormat.spaceFilling = r; };

            EditText et3 = FindViewById<EditText>(Resource.Id.editText3);
            et3.AfterTextChanged += delegate { int r; if (int.TryParse(et3.Text, out r)) OutputFormat.zeroCount = r; };

            EditText et4 = FindViewById<EditText>(Resource.Id.editText4);
            et4.AfterTextChanged += delegate { int r; if (int.TryParse(et4.Text, out r)) OutputFormat.aroundValue = r; };

            EditText et5 = FindViewById<EditText>(Resource.Id.editText5);
            et5.AfterTextChanged += delegate { int r; if (int.TryParse(et5.Text, out r)) OutputFormat.rootsAroundValue = r; };
        }
        public void Calculate(object view,EventArgs e)
        {
            matrixViewController.matrixList.Clear();
            EditText inputText = FindViewById<EditText>(Resource.Id.editText1);
            TextView outputText = FindViewById<TextView>(Resource.Id.Text_RootsOutput);
            string text = inputText.Text;

            SimplexMatrix matrix = new SimplexMatrix(TextToArray(text));
            outputText.Text = SimplexMatrix.SimplifyToEnd(matrix, matrixViewController.AddMatrix).ToString();
        }

        public static double[,] TextToArray(string text)
        {
            List<string> lines = text.Split((char)10,'@','|').ToList();
            List<List<double>> words = lines.ConvertAll(l => l.Split(' ').ToList().ConvertAll(w => double.Parse(w)));
            double[,] result = new double[words.Count, words[0].Count];
            for (int y = 0; y < words.Count; y++)
            {
                for (int x = 0; x < words[0].Count; x++)
                {
                    result[y, x] = words[y][x];
                }
            }
            return result;
        }
        public class MatrixViewController
        {
            public TextView Text_MatrixOutput;
            public List<string> matrixList = new List<string>();
            public int selectedMatrix;
            public MatrixViewController(TextView Text_MatrixOutput)
            {
                this.Text_MatrixOutput = Text_MatrixOutput;
            }
            public void AddMatrix(SimplexMatrix matrix)
            {
                matrixList.Add(matrix.ToString());
                if (matrixList.Count == 1) Text_MatrixOutput.Text = matrix.ToString();
            }
            public void ButtonClick_Previous(object sender,EventArgs e)
            {
                selectedMatrix = System.Math.Max(0, selectedMatrix - 1);
                Text_MatrixOutput.Text = matrixList[selectedMatrix];
            }
            public void ButtonClick_Next(object sender, EventArgs e)
            {
                selectedMatrix = System.Math.Min(matrixList.Count - 1, selectedMatrix + 1);
                Text_MatrixOutput.Text = matrixList[selectedMatrix];
            }
        }
    }
}

