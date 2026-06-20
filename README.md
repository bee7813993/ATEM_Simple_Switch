# ATEM_Simple_Switch

Blackmagic ATEM Switch をコマンドラインからシンプルに切り替える Windows アプリケーション

## 対応 SDK

- Blackmagic_ATEM_Switchers_SDK_10.2.1

## セットアップ

### 前提条件

- Windows OS
- Visual Studio 2019 以上
- Blackmagic_ATEM_Switchers_SDK_10.2.1

### インストール手順

1. **SDK をダウンロード**
   - [Blackmagic Design のダウンロードページ](https://www.blackmagicdesign.com/jp/developer)から `Blackmagic_ATEM_Switchers_SDK_10.2.1` をダウンロード

2. **Program.cs を差し替え**
   - SDK の以下のパスにある `Program.cs` を、このリポジトリの `Program.cs` で置き換えます
   ```
   Blackmagic_ATEM_Switchers_SDK_10.2.1\
   Windows\Samples\SimpleSwitcherExampleCSharp\Program.cs
   ```

3. **ビルド**
   - `SimpleSwitcherExampleCSharp.sln` を Visual Studio で開く
   - ビルド（Build → Build Solution）
   - 出力: `SimpleSwitcherExampleCSharp.exe`

## 使い方

### 基本的な実行方法

```
SimpleSwitcherExampleCSharp.exe <入力ID> [トランジション種別] [IPアドレス]
```

### パラメータ

| パラメータ | 必須 | 説明 | 例 |
|-----------|------|------|-----|
| `<入力ID>` | ○ | 切り替え先の入力ポート番号 | `1`, `2`, `3` |
| `[トランジション種別]` | × | `wipe` または `mix` （デフォルト: `wipe`） | `wipe`, `mix` |
| `[IPアドレス]` | × | ATEM の IP アドレス（デフォルト: USB/自動接続） | `192.168.10.240` |

### 実行例

**例1: 入力2に切り替え（wipe トランジション、USB 接続）**
```
SimpleSwitcherExampleCSharp.exe 2
```

**例2: 入力2に切り替え（mix トランジション、USB 接続）**
```
SimpleSwitcherExampleCSharp.exe 2 mix
```

**例3: 入力2に切り替え（wipe トランジション、IP 指定）**
```
SimpleSwitcherExampleCSharp.exe 2 wipe 192.168.10.240
```

**例4: 入力3に切り替え（mix トランジション、IP 指定）**
```
SimpleSwitcherExampleCSharp.exe 3 mix 192.168.1.100
```

## 機能

- ✅ コマンドラインからの入力切り替え
- ✅ Wipe トランジション対応
- ✅ Mix トランジション対応
- ✅ USB 自動接続（引数なし）
- ✅ 固定 IP による接続
- ✅ トランジション完了検知
- ✅ エラーハンドリング
- ✅ 詳細なエラーメッセージ

## トランジション種別

### Wipe（ワイプ）
- パターン: Rectangle Iris（矩形アイリス）
- フレームレート: 60 fps
- 視覚的な効果のある切り替え

### Mix（ミックス）
- クロスフェード風の切り替え
- フレームレート: 30 fps
- スムーズな暗転による切り替え

## 接続方法

### USB 接続（自動接続）
```
SimpleSwitcherExampleCSharp.exe 2
```
ATEM がUSB で接続されている場合、自動的に接続します。

### ネットワーク接続（固定 IP）
```
SimpleSwitcherExampleCSharp.exe 2 wipe 192.168.10.240
```
指定した IP アドレスの ATEM に接続します。

## エラーメッセージ

| メッセージ | 原因 | 対処 |
|-----------|------|------|
| `エラー: 入力ID '...' は数値ではありません。` | 入力ID が数値でない | 数値を指定してください |
| `エラー: トランジション種別 '...' は無効です。` | トランジション種別が不正 | `wipe` または `mix` を指定してください |
| `ATEMへの接続に失敗しました。` | ATEM に接続できない | USB ケーブルか IP アドレスを確認してください |
| `エラー: 入力ID ... は見つかりませんでした。` | 指定した入力ポートが存在しない | ATEM が持つ入力ポート番号を確認してください |
| `指定した入力はすでに Program 側です。` | 既に指定した入力が表示中 | 処理は正常に完了しました |

## 戻り値

| コード | 説明 |
|-------|------|
| `0` | 処理成功 |
| `1` | エラー発生 |

## 対応 ATEM モデル

このアプリケーションは、Blackmagic_ATEM_Switchers_SDK_10.2.1 でサポートされているすべての ATEM モデルで動作します。

## ライセンス

Blackmagic Design Copyright (c) 2018 によるライセンスの下で提供されます。
