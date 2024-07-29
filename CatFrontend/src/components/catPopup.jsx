import { useCatStore } from "../state/catItemStore";
import { useEffect, useState } from "react";
function CatPopup({apiUrl}){
   
    const catItem = useCatStore((state) => state.selectedCatItem);
    let tempCatRatingScore=useCatStore((state) => state.selectedCatItem.catScore);
    let catImgUrl=apiUrl+catItem.catImgName;
    const [rating,setRating] = useState(tempCatRatingScore);
  
  useEffect(()=>{
   
    setRating(tempCatRatingScore);
    return ()=>setRating(0);
  },[tempCatRatingScore])


  function updateCatItem(){
  
    fetch("".concat(import.meta.env.VITE_CAT_API,"api/CatItems/")+catItem.id,
    {
      mode: "cors",
      method: "PUT", 
      headers: {
        "Content-Type": "application/json",
      },
      credentials: "same-origin",
      body: JSON.stringify({...catItem,catScore:rating}), // body data type must match "Content-Type" header
    }
  ).then((e)=>{
    if(e.status==204){
     console.log("başarılı");
      return;
    }
  })
  }


  function deleteCatItem(){
    fetch("".concat(import.meta.env.VITE_CAT_API,"api/CatItems/")+catItem.id,
    {
      mode: "cors",
      method: "DELETE", 
      headers: {
        "Content-Type": "application/json",
      },
      credentials: "same-origin",
    }).then((e)=>{
      if(e.status==204){
       console.log("başarılı");
        return;
      }
    })
  }

    return(
        <div  className="modal fade " id="exampleModal" tabIndex="-1" aria-labelledby="exampleModalLabel" aria-hidden="true">
  <div className="modal-dialog modal-lg modal-dialog-centered ">
    <div className="modal-content rounded-lg overflow-hidden bg-dark">
   
        
        <button type="button" className="btn-close position-absolute" data-bs-dismiss="modal" aria-label="Close"></button>
      
      <div className="d-flex align-items-stretch">
        <img src={catImgUrl} className="catPopupImg w-50" ></img>
        <div className="w-50 d-flex flex-column justify-content-around">
            <h1>{catItem.catScore} / 10 ⭐</h1>
            {rating}
            <input type="range" className="form-range" min="0" max="10" step="1" value={rating} onChange={(e)=>setRating(e.target.value)}/>
            <button className="btn btn-primary" onClick={()=>updateCatItem()}>Save</button>
            <button className="btn btn-danger" onClick={()=>deleteCatItem()}>Delete</button>
        </div>
      
      </div>
      
    </div>
  </div>
</div>
    );
}

export default CatPopup;