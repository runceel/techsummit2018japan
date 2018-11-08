# Device リストの取得
GET https://graph.microsoft.com/beta/me/devices

# アプリの起動
POST https://graph.microsoft.com/beta/me/devices/6575f83f-3da4-5172-8771-c803e8bec8c4/commands
{ "type" : "LaunchUri", "payload" : {"uri":"techsummit2018demo:?message=hoge"}}

# 操作
Package family name: 3f2f7893-6f56-4628-9ddc-9131006cc39f_5ppbtxp1sbcde