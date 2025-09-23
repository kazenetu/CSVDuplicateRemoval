namespace CSVDuplicateRemoval;
using System.Text;

class Program
{
    /// <summary>
    /// エントリメソッド
    /// </summary>
    static void Main(string[] args)
    {
        // パラメータが不足の場合はヘルプ表示
        if (args.Length < 2)
        {
            Console.WriteLine("dotnet run \"入力CSVファイル\" \"カラム変換リスト（カンマ区切り）\"");
            Console.WriteLine("  入力CSVファイル:読み込みCSVファイル");
            Console.WriteLine("  カラム変換リスト（カンマ区切り）:カンマ区切りで下記を選択");
            Console.WriteLine("    ・None:変換なし");
            Console.WriteLine("    ・Name:氏名  全角・半角スペース取り除き");
            Console.WriteLine("    ・Address:住所  全角英数字→半角英数字、番地→-");
            Console.WriteLine("    ・PostNo:郵便番号  全角数字→半角数字、-・ー除外");
            return;
        }

        // 入力パラメータ取得
        var csvFilePath = args[0];
        var columns = args[1].Split(',');

        // 入力チェック：CSVファイル
        if (!File.Exists(csvFilePath))
        {
            Console.WriteLine($"入力CSVファイル[{csvFilePath}]が存在しません");
            return;
        }

        //　CSVファイルを読み込み
        var csvList = LoadCSV(csvFilePath);

        // 重複確認用にCSV読み込み結果を変換
        var convertResult = Convert(csvList,columns);

        // ファイル出力
        var outputPath = Environment.CurrentDirectory + "/Output";
        var contents = new StringBuilder();

        Console.WriteLine($"変換結果：重複分");
        var tempResult = string.Empty;
        foreach(var result in convertResult.OrderBy(item => item))
        {
            if(tempResult == result)
            {
                Console.WriteLine($"> {result}");
            }
            else
            {
                contents.AppendLine(result);
            }
            tempResult = result;
        }

        CreateFile(outputPath, "convert.csv", contents.ToString());
    }

    /// <summary>
    /// CSVファイルの読み込み処理
    /// </summary>
    /// <param name="csvFilePath">CSVパス</param>
    /// <returns>読み込み結果</returns>
    private static List<string[]> LoadCSV(string csvFilePath)
    {
        var result = new List<string[]>();
        using (StreamReader sr = new StreamReader(csvFilePath))
        {
            string? line;
            while ((line = sr.ReadLine()) != null)
            {
                var array = line.Split(',').Select(col => col.Replace("\"", string.Empty)).ToArray();
                result.Add(array);
            }
        }

        return result;
    }

    /// <summary>
    /// CSV読み込み結果の変換結果取得
    /// </summary>
    /// <param name="csvList">CSV読み込み結果</param>
    /// <param name="convertColmus">変換カラムリスト</param>
    /// <returns>変換結果文字列リスト</returns>
    private static List<string> Convert(List<string[]> csvList, string[] convertColmus)
    {
        var result = new List<string>();

        foreach (var row in csvList)
        {
            var rowResult = new StringBuilder();
            var colIndex = 0;
            foreach (var col in row)
            {
                var colResult = col;
                if (colIndex < convertColmus.Length)
                {
                    switch (convertColmus[colIndex])
                    {
                        case "Name":
                            colResult = col.Replace(" ", string.Empty).Replace("　", string.Empty);
                        break;
                        case "Address":
                            colResult = ConvertAddress(col);
                        break;
                        case "PostNo":
                            colResult = ConvertPosetNo(col);
                        break;
                    }
                }
                rowResult.Append($"{colResult},");

                colIndex++;
            }

            result.Add(rowResult.ToString());
        }

        return result;
    }

    /// <summary>
    /// 住所の書式統一変換
    /// </summary>
    /// <param name="target">変換元文字列</param>
    /// <returns><変換結果/returns>
    private static string ConvertAddress(string target)
    {
        return target
            .Replace("０", "0")
            .Replace("１", "1")
            .Replace("２", "2")
            .Replace("３", "3")
            .Replace("４", "4")
            .Replace("５", "5")
            .Replace("６", "6")
            .Replace("７", "7")
            .Replace("８", "8")
            .Replace("９", "9")
            .Replace("番地", "-");
    }

    /// <summary>
    /// 郵便番号の書式統一変換
    /// </summary>
    /// <param name="target">変換元文字列</param>
    /// <returns><変換結果/returns>
    private static string ConvertPosetNo(string target)
    {
        return target
            .Replace("０", "0")
            .Replace("１", "1")
            .Replace("２", "2")
            .Replace("３", "3")
            .Replace("４", "4")
            .Replace("５", "5")
            .Replace("６", "6")
            .Replace("７", "7")
            .Replace("８", "8")
            .Replace("９", "9")
            .Replace("ー", string.Empty)
            .Replace("-", string.Empty);
    }

    /// <summary>
    /// ファイル書き出し
    /// </summary>
    /// <param name="path">書き出しパス</param>
    /// <param name="fileName">書き出しファイル名</param>
    /// <param name="contents">書き出し内容(ソースコード)</param>
    private static void CreateFile(string path, string fileName, string contents)
    {
        // ディレクトリ作成
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        // ファイル作成
        File.WriteAllText(Path.Combine(path, fileName), contents);
    }
}
