# CSVDuplicateRemoval
CSVファイルから重複を削除する機能

## コマンド
```sh
dotnet run "入力CSVファイル" "カラム変換リスト（カンマ区切り）"
```  

**変換値**
|設定|概要|補足説明|
|---|---|----|
|None|変換なし||
|Name|氏名|全角・半角スペースを取り除く|
|Address|住所|書式統一：全角数字→半角数字、番地→-|
|PostNo|郵便番号|書式統一：全角数字→半角数字、-・ーを取り除き|

### コマンド例
```sh
dotnet run --project CSVDuplicateRemoval/CSVDuplicateRemoval.csproj "InputFiles/test.csv" "Name,Address,PostNo"
```