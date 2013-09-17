function HandleStartPostback(url) {
    document.body.style.cursor = "wait";
    if (MasterLoadingPanel != null) {
        MasterLoadingPanel.Show();
    }
}

function HandleEndPostback() {
    document.body.style.cursor = "auto";
    if (MasterLoadingPanel != null) {
        MasterLoadingPanel.Hide();
    }
}