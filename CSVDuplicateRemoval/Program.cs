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
    }
}
