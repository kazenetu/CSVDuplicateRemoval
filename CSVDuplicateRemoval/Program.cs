namespace CSVDuplicateRemoval;

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

        Console.WriteLine($"入力ファイル{csvFilePath}]");
        foreach (var line in csvList)
        {
            Console.Write("> ");
            foreach (var column in line)
            {
                Console.Write($"{column}, ");
            }
            Console.WriteLine();
        }

        Console.WriteLine($"カラム変換リスト");
        foreach(var column in columns)
        {
            Console.WriteLine($"> {column}");
        }
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
}
