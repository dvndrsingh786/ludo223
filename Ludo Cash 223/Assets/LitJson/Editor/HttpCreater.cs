//----------------------------------------------
// LitJson Ruler
// © 2015 yedo-factory
//----------------------------------------------
using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;

namespace LJR
{
    /// <summary>
	/// HttpCreater
    /// </summary>
	public class HttpCreater : EditorWindow
    {
		public static readonly string AssetName         = "LJR";
        public static readonly string HttpInterfacePath = Application.dataPath + "/" + AssetName + "/Editor/HttpInterface.xls";
        public static readonly string SettingPath       = Application.dataPath + "/" + AssetName + "/Scripts/Common/Setting.cs";
        public static readonly string HttpDir           = Application.dataPath + "/" + AssetName + "/Scripts/Http/";
		public static readonly string HttpPath          = HttpDir + "{0}.cs";

        public static readonly string TagBaseURL           = "BaseURL";
        public static readonly string TagURL               = "URL";
        public static readonly string TagRequestClass      = "RequestClass";
        public static readonly string TagResponceClass     = "ResponceClass";
        public static readonly string TagRequestParameter  = "RequestParameter";
        public static readonly string TagResponceParameter = "ResponceParameter";
        public static readonly string TagEnd               = "End";

        public static readonly int MoveCellValue     = 5;
        public static readonly int MoveCellHierarchy = 25;

        public static readonly string TypeGet         = "get";
        public static readonly string TypePost        = "post";

        public static readonly string TypeString      = "string";
        public static readonly string TypeStringArray = "string[]";
        public static readonly string TypeInt         = "int";
        public static readonly string TypeIntArray    = "int[]";
        public static readonly string TypeLong        = "long";
        public static readonly string TypeLongArray   = "long[]";
        public static readonly string TypeFloat       = "float";
        public static readonly string TypeFloatArray  = "float[]";
        public static readonly string TypeDouble      = "double";
        public static readonly string TypeDoubleArray = "double[]";
        public static readonly string TypeBool        = "bool";
        public static readonly string TypeBoolArray   = "bool[]";
        public static readonly string TypeClass       = "class";
        public static readonly string TypeClassArray  = "class[]";

		public static readonly string ExtCs        = ".cs";
        public static readonly string Tab          = "\t";
        public static readonly string ParameterLog = ":{0}({1}, {2})";

        /// <summary>
        /// エラーログ
        /// </summary>
        private static string log = "";

        /// <summary>
        /// 通信クラス作成
        /// </summary>
		[MenuItem("Tools/LJR/Create")]
        public static void Create()
        {
            try
            {
                // Excelオープン
				EditorUtility.DisplayProgressBar("HttpCreater", "NowCreating...", 1f);
				log = "open excel " + HttpInterfacePath;
				using (FileStream fs = new FileStream(HttpInterfacePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    // 出力ファイル初期化
                    log = "init files";
                    if (File.Exists(SettingPath)) { File.Delete(SettingPath); }
                    if (!Directory.Exists(HttpDir)) { Directory.CreateDirectory(HttpDir); }
                    foreach (string path in Directory.GetFiles(HttpDir))
                    {
						if (Path.GetExtension(path) == ExtCs) { File.Delete(path); }
                    }

                    // 設定シート読み込み
                    string baseUrl = "";
                    IWorkbook book = new HSSFWorkbook(fs);
                    ISheet sheet = book.GetSheetAt(0);
                    log = sheet.SheetName;
                    for (int rowIdx = 0; rowIdx <= sheet.LastRowNum; rowIdx++)
                    {
                        string tag = GetCell(sheet, rowIdx, 0);
                        if (tag == "") { continue; }

                        if (tag == TagBaseURL)
                        {
                            // ベースURL取得
                            log += ":get BaseURL";
                            baseUrl = GetCell(sheet, rowIdx, MoveCellValue);
                            break;
                        }
                    }

                    // 設定情報出力
                    log += ":create Setting";
                    using (StreamWriter sw = new StreamWriter(SettingPath, false, Encoding.UTF8))
                    {
                        sw.WriteLine("//----------------------------------------------");
						sw.WriteLine("// LitJson Ruler");
                        sw.WriteLine("// © 2015 yedo-factory");
                        sw.WriteLine("// auto-generated");
                        sw.WriteLine("//----------------------------------------------");
						sw.WriteLine("namespace LJR");
                        sw.WriteLine("{");
                        sw.WriteLine(Tab + "public static class Setting");
                        sw.WriteLine(Tab + "{");
                        sw.WriteLine(Tab + Tab + "public static string BaseUrl = \"" + baseUrl + "\";");
                        sw.WriteLine(Tab + "}");
                        sw.WriteLine("}");
                    }

                    // 通信シート読み込み
                    List<string> allResponces = new List<string>();
                    for (int sheetIdx = 1; sheetIdx < book.NumberOfSheets; sheetIdx++)
                    {
                        sheet = book.GetSheetAt(sheetIdx);
                        log = sheet.SheetName;

                        string url = "";
                        string requestClass = "";
                        string responceClass = "";
                        Dictionary<string, string> requests = new Dictionary<string, string>();
                        Dictionary<string, Dictionary<string, string>> responces = new Dictionary<string, Dictionary<string, string>>();

                        // 先頭セルからタグ情報を解析
                        for (int rowIdx = 0; rowIdx <= sheet.LastRowNum; rowIdx++)
                        {
                            string tag = GetCell(sheet, rowIdx, 0);
                            if (tag == "") { continue; }

                            if (tag == TagURL)
                            {
                                // URL
                                log += ":get URL";
                                url = GetCell(sheet, rowIdx, MoveCellValue);
                            }
                            else if (tag == TagRequestClass)
                            {
                                // リクエストクラス名
                                log += ":get RequestClass";
                                requestClass = GetCell(sheet, rowIdx, MoveCellValue);
                            }
                            else if (tag == TagResponceClass)
                            {
                                // レスポンスクラス名
                                log += ":get ResponceClass";
                                responceClass = GetCell(sheet, rowIdx, MoveCellValue);
                            }
                            else if (tag == TagRequestParameter)
                            {
                                // リクエストパラメータ
                                log += ":get RequestParameter";
                                if (GetCell(sheet, rowIdx + 1, 0) != "" && GetCell(sheet, rowIdx + 2, 0) != "") // 次の次のセルまで記述がある場合、パラメータあり
                                {
                                    // リクエストクラス作成
                                    rowIdx += 2;
                                    CreateRequestClass(sheet, requests, requestClass, ref rowIdx, 0);
                                }
                            }
                            else if (tag == TagResponceParameter)
                            {
                                // レスポンスパラメータ
                                log += ":get ResponceParameter";
                                responces.Add(responceClass, new Dictionary<string, string>());
                                allResponces.Add(responceClass);
                                if (GetCell(sheet, rowIdx + 1, 0) != "" && GetCell(sheet, rowIdx + 2, 0) != "") // 次の次のセルまで記述がある場合、パラメータあり
                                {
                                    // レスポンスクラス作成
                                    rowIdx += 2;
                                    CreateResponceClass(sheet, allResponces, responces, responceClass, ref rowIdx, 0);
                                }
                                break;
                            }
                        }

                        // 通信クラス出力
                        log += ":create Http";
                        using (StreamWriter sw = new StreamWriter(string.Format(HttpPath, requestClass), false, Encoding.UTF8))
                        {
                            sw.WriteLine("//----------------------------------------------");
							sw.WriteLine("// LitJson Ruler");
                            sw.WriteLine("// © 2015 yedo-factory");
                            sw.WriteLine("// auto-generated");
                            sw.WriteLine("//----------------------------------------------");
                            sw.WriteLine("using UnityEngine;");
                            sw.WriteLine("using System;");
                            sw.WriteLine("");
							sw.WriteLine("namespace LJR");
                            sw.WriteLine("{");
                            sw.WriteLine(Tab + "public class " + requestClass + " : Request<" + responceClass + ">");
                            sw.WriteLine(Tab + "{");

                            sw.WriteLine(Tab + Tab + "public static readonly string URL = \"" + url + "\";");
                            sw.WriteLine("");
                            if (requests.Count > 0)
                            {
                                foreach (var pair in requests)
                                {
                                    sw.WriteLine(Tab + Tab + "public string " + pair.Key + " = \"\";");
                                }
                            }
                            sw.WriteLine(Tab + Tab + "public Action<" + requestClass + ", " + responceClass + "> onFinish = null;");
                            sw.WriteLine("");

                            string arg = (requests.Count > 0) ? requests.Select((a) => TypeString + " " + a.Key + ", ").Aggregate((a, b) => a + b) : "";
                            sw.WriteLine(Tab + Tab + "public void Send(" + arg + "Action<" + requestClass + ", " + responceClass + "> onFinish)");
                            sw.WriteLine(Tab + Tab + "{");
                            foreach (var pair in requests)
                            {
                                sw.WriteLine(Tab + Tab + Tab + "this." + pair.Key + " = " + pair.Key + ";");
                            }
                            sw.WriteLine(Tab + Tab + Tab + "this.onFinish = onFinish;");
                            sw.WriteLine(Tab + Tab + Tab + "Send();");
                            sw.WriteLine(Tab + Tab + "}");
                            sw.WriteLine("");

                            sw.WriteLine(Tab + Tab + "public override void Send()");
                            sw.WriteLine(Tab + Tab + "{");
							sw.WriteLine(Tab + Tab + Tab + "get.Clear ();");
							sw.WriteLine(Tab + Tab + Tab + "post.Clear ();");
                            foreach (var pair in requests)
                            {
                                sw.WriteLine(Tab + Tab + Tab + pair.Value + ".Add(\"" + pair.Key + "\", " + pair.Key + ");");
                            }
                            sw.WriteLine(Tab + Tab + Tab + "Http.Send(this, URL, get, post, onFinish);");
                            sw.WriteLine(Tab + Tab + "}");
                            sw.WriteLine(Tab + "}");

                            foreach (var pair0 in responces)
                            {
                                sw.WriteLine("");
                                sw.WriteLine(Tab + "[Serializable]");
                                sw.WriteLine(Tab + "public class " + pair0.Key + ((pair0.Key == responceClass) ? " : Responce" : ""));
                                sw.WriteLine(Tab + "{");
                                foreach (var pair1 in pair0.Value)
                                {
                                    sw.WriteLine(Tab + Tab + "public " + pair1.Value + " " + pair1.Key + ";");
                                }
                                sw.WriteLine(Tab + "}");
                            }
                            sw.WriteLine("}");
                        }
                    }
                }

                // ファイル情報を更新して終了
                AssetDatabase.Refresh();
                Debug.Log("Finish Shimauma！");
            }
            catch(Exception e)
            {
                Debug.LogError(log);
                Debug.LogException(e);
            }
            finally
            {
                EditorUtility.ClearProgressBar();
            }
        }

        /// <summary>
        /// リクエストクラス作成
        /// </summary>
        /// <param name="sheet">Excelシート</param>
        /// <param name="requests">リクエストクラスリスト</param>
        /// <param name="className">クラス名</param>
        /// <param name="rowIdx">Rowインデックス</param>
        /// <param name="cellIdx">Cellインデックス</param>
        private static void CreateRequestClass(ISheet sheet, Dictionary<string, string> requests, string className, ref int rowIdx, int cellIdx)
        {
            for (; rowIdx <= sheet.LastRowNum; rowIdx++)
            {
                try
                {
                    string variable = GetCell(sheet, rowIdx, cellIdx);
                    string type = GetCell(sheet, rowIdx, cellIdx + MoveCellValue);
                    if (variable != "")
                    {
                        // 上から順に保存
                        requests.Add(variable, type);
                    }
                    else
                    {
                        // 空セルになったら終了
                        break;
                    }
                }
                catch (Exception e)
                {
                    log += string.Format(ParameterLog, className, (rowIdx + 1), (cellIdx + 1));
                    throw e;
                }
            }
        }

        /// <summary>
        /// レスポンスクラス作成
        /// </summary>
        /// <param name="sheet">Excelシート</param>
        /// <param name="allResponces">全レスポンスクラスリスト</param>
        /// <param name="responces">レスポンスクラスリスト</param>
        /// <param name="className">クラス名</param>
        /// <param name="rowIdx">Rowインデックス</param>
        /// <param name="cellIdx">Cellインデックス</param>
        /// <param name="isInnerClass">インナークラスの場合 true</param>
        private static void CreateResponceClass(ISheet sheet, List<string> allResponces, Dictionary<string, Dictionary<string, string>> responces, string className, ref int rowIdx, int cellIdx, bool isInnerClass = false)
        {
            Dictionary<string, string> variables = responces[className];
            for (; rowIdx <= sheet.LastRowNum; rowIdx++)
            {
                try
                {
                    string variable = GetCell(sheet, rowIdx, cellIdx);
                    string type = GetCell(sheet, rowIdx, cellIdx + MoveCellValue);

                    // インナークラスの場合は終了判定
                    if (isInnerClass)
                    {
                        // 親クラス名が変化した場合に終了
                        string parentVariable = GetCell(sheet, rowIdx, cellIdx - MoveCellHierarchy);
                        if(parentVariable != "")
                        {
                            string parentClassName = Char.ToUpper(parentVariable[0]) + parentVariable.Substring(1);
                            if (className != parentClassName) 
                            {
                                rowIdx--; // 1つ進んでるので1個前に戻す
                                break; 
                            }
                        }
                    }

                    if (variable == TagEnd)
                    {
                        // Endタグで全終了
                        break;
                    }
                    else if (variable == "" || type == "")
                    {
                        // 空セルは飛ばす
                        continue;
                    }
                    else if (type != TypeClass && type != TypeClassArray)
                    {
                        // 通常変数
                        variables.Add(variable, type);
                    }
                    else
                    {
                        // インナークラス
                        string innerClassName = Char.ToUpper(variable[0]) + variable.Substring(1); // クラス名はPascal形式(変数名との重複を避ける)
                        string innerType = innerClassName + ((type == TypeClassArray) ? "[]" : ""); // 配列の場合記号追加
                        variables.Add(variable, innerType);
                        if (!allResponces.Contains(innerClassName)) // すでに定義済みのクラスは除外
                        {
                            // 階層移動してインナークラス作成へ
                            responces.Add(innerClassName, new Dictionary<string, string>());
                            allResponces.Add(innerClassName);
                            CreateResponceClass(sheet, allResponces, responces, innerClassName, ref rowIdx, cellIdx + MoveCellHierarchy, true);
                        }
                    }
                }
                catch (Exception e)
                {
                    log += string.Format(ParameterLog, className, (rowIdx + 1), (cellIdx + 1));
                    throw e;
                }
            }
        }

        /// <summary>
        /// Excelセル値取得
        /// </summary>
        /// <param name="sheet">Excelシート</param>
        /// <param name="rowIdx">Rowインデックス</param>
        /// <param name="cellIdx">Cellインデックス</param>
        // <returns>Excelセル値。取得に失敗した場合は空文字</returns>
        private static string GetCell(ISheet sheet, int rowIdx, int cellIdx)
        {
            IRow row = sheet.GetRow(rowIdx);
            ICell cell = (row != null) ? row.GetCell(cellIdx) : null;
            return (cell != null) ? cell.StringCellValue : "";
        }
    }
}
