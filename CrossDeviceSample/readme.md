# README

## 試し方

CrossDeviceSample プロジェクトと CrossDeviceSample.UWP プロジェクトをデバッグ実行する。
Visual Studio の出力ウィンドウに Family Name のログが出るので控える。

### CrossDeviceSample.AppService プロジェクトの編集

`CommunicationService.cs` の `ClientFamilyName` の値を Desktop app's family name の値にする。

### CrossDeviceSample.WPF プロジェクトの編集

`Consts.cs` の `CommunicationServiceHost` の値を UWP app's family name の値にする。 



## Microsoft Graph explorer を開く
https://developer.microsoft.com/ja-jp/graph/graph-explorer

PC に紐づいているマイクロソフトアカウントでサインインして以下のアクセス許可にチェックを入れて再度サインインする。

```
Device.Read
Device.Command
```

### デバイスの ID を取得する

GET で `https://graph.microsoft.com/beta/me/devices` を叩くとデバイスのリストが出てくるので PC の ID を控えておく。

### アプリの起動

POST で `https://graph.microsoft.com/beta/me/devices/先ほど取得したデバイスのID/command` に対して要求本文に以下の JSON を指定して起動するとアプリが起動する。

```json
{
  "type": "LaunchUri",
  "payload": {
    "uri": "crossdevicesample:?message=LaunchFromGraphExplorer"
  }
}
```

### コマンドの送信

アプリが起動したら要求本文を以下の値に変更することでアプリのテキストを更新できる。

```json
{
  "type": "appService",
  "payload": { "message": "Update from graph explorer" },
  "packageFamilyName": "8954fcae-b3bb-4921-a00a-713ba593aa13_5ppbtxp1sbcde",
  "appServiceName": "CommunicationService"
}
```

## 仕組み

Microsoft Graph API の Cross-device experience という API を使うことで Microsoft Graph API(REST API) 経由で同じマイクロソフトアカウントに紐づいているデバイスに対して操作ができる。
現状、アプリの起動をアプリサービスに対してコマンドを送ることが出来ます。ここではコマンドを送った結果は受けてませんがポストバック URL なども指定できるので恐らく受け取れるはず。

### アプリの起動について

UWP アプリ（含む Dekstop Bridge）はプロトコルに対応出来ます。
原理的にはプロトコルに対応しているアプリは全て Microsoft Graph API から起動することが出来ます。

今回は WPF アプリを Desktop Bridge を使って UWP 化してプロトコル対応にして起動できるようにしました。

### コマンドの送信について

コマンドへの対応は、Desktop Bridge アプリ単品ではなく App Service を持った UWP アプリを別に作成しています。
Microsoft Graph API からはリモートアクセスを許可している App Service に対してコマンドを送ることが出来ます。今回は、WPF アプリの起動時に UWP アプリの App Service に接続しにいって応答を待っています。

App Service に Graph API 経由でコマンドが来たら、それを接続がつながっている WPF アプリに対して転送しています。
転送されたメッセージをもとに WPF アプリでは画面のテキストを更新しています。

