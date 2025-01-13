private async void PlayButton_Click(object sender, EventArgs e)
{
    // MousePlayerのインスタンスを作成
    var mousePlayer = new MousePlayer(mouseActions, keyboardActions);

    // PlayActionsAsyncメソッドを呼び出してアクションを再生
    await mousePlayer.PlayActionsAsync();
} 