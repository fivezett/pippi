pippi - v1.0

Win32API経由で普通のアプリケーションをPIP的な使い方をするソフトウェアです。
ClusterとかVRCで遊びながら作業してるときに画面の有効活用したいなと思って作りました。

α版ですので不具合とかには目を瞑ってください。

プルダウンリストからアプリケーションを選択してPIPを押すとPIP風ウィンドウに切り替わります。
unPIPを押すと戻ります。

仕様としては、
PIP時に最前面固定、タイトルバーなしウィンドウとして扱わせていて、
unPIP時にはタイトルバー、_□Xボタンがあるウィンドウとして扱う。
と言った挙動になっているためソフトウェアによっては、元通りにならない可能性があります。
状態は変更されたウィンドウが終了するまでしか続きませんので、再起動すれば治ります。

制作 : @FiveZett