import { useState, useEffect } from 'react'
import { create } from 'zustand'
import CatImg from './components/catImg';
import CatPopup from './components/catPopup';


function App() {
  
  const [catBasket, setCatBasket] = useState([]);
  var list=[];

  function fetchAndListCatImgs(){
    fetch("".concat(import.meta.env.VITE_CAT_API,"api/CatItems")).then((res)=>res.json()).then((data)=>{
      setCatBasket([...data]);  
   });
  }

  useEffect(() => {
    fetchAndListCatImgs();
  },[]);


  function getNewCatImg(){
    //rabbit mq



    let spinner=document.getElementById("spinner");
    spinner.classList.remove("d-none");
    fetch("".concat(import.meta.env.VITE_CAT_API,"getnewcatimage")).then((res)=>{
      if(res.status==200){
        spinner.classList.add("d-none");
        fetchAndListCatImgs();
      }
    })
  }
 

  return (
    <>
      <div id='spinner' className="d-flex justify-content-center d-none">
        <div className="spinner-border" role="status">
        </div>
      </div>

        <h1>Hello, Welcome The Cat App </h1>
        <h4>This app development for learning api's basic</h4>

      <button onClick={()=>getNewCatImg()}>Get New Cat Image</button>       
    
      <div >
        
      <CatPopup apiUrl={import.meta.env.VITE_CAT_API} ></CatPopup>
     <div className='catArea'>
          {catBasket.map((e,index)=>{
            return <CatImg  key={index} apiUrl={import.meta.env.VITE_CAT_API} catId={e["id"]} catImgName={e["catImgName"]} catScore={e["catScore"]} ></CatImg>
          
          })}
         </div>
     
      </div>
      <p className="read-the-docs">
        Click on the Vite and React logos to learn more
      </p>
    </>
  )
}

export default App
