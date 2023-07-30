/**
 * @name JackStreamBox
 * @author CodeChrisB
 * @description Describe the basic functions. Maybe a support server link.
 * @version 0.0.1
 */

async function StartStreaming() {
    let vcName = "Jackbot";

    //------Join Server
    let server = document.querySelector(`div[data-list-item-id="guildsnav___697504479834275898"]`);
    server.click()
    //------Join VC
    await new Promise(resolve => setTimeout(resolve, 500));
    let listItem = document.querySelector(`li[data-dnd-name="${vcName}"]`); //Get correct vc
    let vcATag = listItem.querySelector("a"); //get link to join
    vcATag.click();
    //------Open the Stream UI
    await new Promise(resolve => setTimeout(resolve, 100));
    let StreamBtn = document.getElementsByClassName("button-12Fmur enabled-9OeuTA button-ejjZWC lookBlank-FgPMy6 colorBrand-2M3O3N grow-2T4nbg button-12Fmur")[0]
    if (!StreamBtn) return
    StreamBtn.click()
    //------Wait for Stream UI to pop up then start stream
    await new Promise(resolve => setTimeout(resolve, 800));
    let goLiveBtn = document.querySelectorAll(`button[type="submit"]`)
    if (!goLiveBtn) return
    console.log(goLiveBtn)
    goLiveBtn[0].click()
}




module.exports = class {
    start() {
        StartStreaming()
    }
    stop() { }
};
