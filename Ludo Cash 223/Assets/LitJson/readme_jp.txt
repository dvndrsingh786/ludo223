//----------------------------------------------
// LitJson Ruler
// © 2015 yedo-factory
// Version 1.0.0
//----------------------------------------------
LitJson Rulerは、LitJsonを用いた通信のやり取りを、Excelで全て自動化したAssetである！

1.Excelで通信インターフェイスを作成
2.Unity上でExcelデータをパース(ボタン1つ)
3.自動生成されたクラスで通信を実行(コード1行！)

これだけ。

誰でも簡単に導入可能で、更新も容易。Excelなので通信のやり取りも分かりやすい。
複雑な構造の通信データも、LitJson Rulerが全て自動的にクラスを作成してくれます。

LitJson Rulerで、快適なLitJsonライフを・・・！

■使用方法
(1)Project[LJR/Editor/HttpInterface.xls]に通信インターフェイスを記述
(2)Menu[Tools/LJR/Create]で通信クラスを作成
(3)Project[LJR/Scripts/Http/***]に作成された通信クラスをHierarchyに設置し、Sendメソッドで通信実行。Inspectorの[Test]ボタンからUnityEditor上での通信も可能です。通信データはInspectorに表示されるので、[Test]ボタンでの確認であれば、ノーコーディングで通信を行えます！
※Demo通信の動作確認は、[HttpInterface.xls/Setting/BaseURL]を自分の環境に変更して下さい

■バージョン履歴
1.0.0
- 初期バージョン