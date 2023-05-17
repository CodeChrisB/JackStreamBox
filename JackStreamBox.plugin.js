// JavaScript source code
/**
 * @name JackStreamBox
 * @author CodeChrisB
 * @description Describe the basic functions. Maybe a support server link.
 * @version 0.0.1
 */

function StartStreaming() {
  let vcName = "Jackbot";
  //Join VC
  let listItem = document.querySelector(`li[data-dnd-name="${vcName}"]`);
  let vcATag = listItem.getElementsByClassName("mainContent-20q_Hp")[0];
  vcATag.click();
  //Stream Button
  let StreamBtn =document.getElementsByClassName("button-12Fmur enabled-9OeuTA button-ejjZWC lookBlank-FgPMy6 colorBrand-2M3O3N grow-2T4nbg button-12Fmur")[0]
  if(!StreamBtn) return
  StreamBtn.click()

  setTimeout(()=>{
    let goLiveBtn = document.querySelectorAll(`button[type="submit"]`)
    if(!goLiveBtn) return
    console.log(goLiveBtn)
    //when the bot bugs it might open multiple widows thus goLiveBtn is an NodeList
    goLiveBtn[0].click()
    
  }, 1000);
}




module.exports = class {
  start() {
    StartStreaming()
  }
  stop() {}
};
