import { useCatStore } from "../state/catItemStore";

function CatImg({apiUrl,catId,catScore,catImgName}){
    let catImgUrl=apiUrl + catImgName;
    const setCatItem =  useCatStore((state)=>state.setCatItem);
    const aa= useCatStore((state)=>state.selectedCatItem);

   function bruh(){
        setCatItem(
            {id:catId,catImgName:catImgName,catScore:catScore}
        );
       

   }
    return (

        <div className="catCard" onClick={()=>bruh()}  data-bs-toggle="modal" data-bs-target="#exampleModal">
            <h1> {catScore}/10 ⭐</h1>
            <img className='catImg'  src={catImgUrl}></img>
        </div>
    );
}

export default CatImg;