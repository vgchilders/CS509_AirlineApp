import axios from "axios";
import { useEffect } from "react";
import { useState } from "react";

function City() {
  const [response, setresponse] = useState([]);
  const getdata = async () => { //function to get data from backend
      try{
          const res = await axios.get('http://localhost:5218/api/v1/CombinedFlights/search');
          setresponse(res.data);
      }catch(err){
          console.log("error");
      }
  };

  useEffect( () => { //saying "on load run function"
      getdata();
  } ,[]);

  if(response){
    return (<h1> Flight data: {JSON.stringify(response)} </h1>);
  }else{
    return (<h1> No flight data available </h1>);
  }
}

export default City
