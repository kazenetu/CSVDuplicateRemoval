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

        // 重複チェックと出力用CSV文字列の追加
        var duplicateList = new List<string>();
        var tempResult = string.Empty;
        foreach(var result in convertResult.OrderBy(item => item.convertResult))
        {
            // 行文字列を作成
            var rowResult = new StringBuilder();
            var srcArray = csvList[result.srcIndex];
            foreach (var col in srcArray)
            {
                rowResult.Append($"{col},");
            }

            if(tempResult == result.convertResult)
            {
                // 前回と一致場合は重複
                duplicateList.Add(rowResult.ToString());
            }
            else
            {
                // 前回と一致しない場合はCSV出力対象
                contents.AppendLine(rowResult.ToString());
            }
            tempResult = result.convertResult;
        }

        //　重複表示
        if (duplicateList.Any())
        {
            Console.WriteLine($"{Environment.NewLine}重複が存在します！");
            foreach (var duplicateItemm in duplicateList)
            {
                Console.WriteLine($"  > {duplicateItemm}");
            }
            Console.WriteLine();
        }

        // CSV出力
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
    private static List<(int srcIndex, string convertResult)> Convert(List<string[]> csvList, string[] convertColmus)
    {
        var result = new List<(int srcIndex, string convertResult)>();

        var rowIndex = 0;
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

            result.Add((rowIndex, rowResult.ToString()));
            rowIndex++;
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
