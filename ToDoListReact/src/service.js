import axios from 'axios';

axios.defaults.baseURL = 'http://localhost:5123';

axios.interceptors.response.use(
    response => {
      return response;
    },
    error => {
      console.error('Axios error:', error);
      return Promise.reject(error);
    }
  );
export default {
  getTasks: async () => {
    const result = await axios.get('/items')    
    return result.data;
  },

  addTask: async(name)=>{ 
    return await axios.post('/items',{
        "id": 0,
        "name": name,
        "isComplete": false
      },{
        headers: {
          'Content-Type': 'application/json'
        }}).data;
  },

  setCompleted: async(id, name, isComplete)=>{
    return await axios.put(`/items/${id}`,{
        "id":id,
        "name":name,
        "isComplete":isComplete
    },{
        headers: {
          'Content-Type': 'application/json'
        }})
  },

  deleteTask:async(id)=>{
    return await axios.delete(`/items/${id}`).data
    
  }
};
