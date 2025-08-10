window.addEventListener('DOMCountentloaded', (event) => {
    getVisitcount();
})    

const functionApi = '';

const getvisitcount = () => {
    let count = 30;
    fetch(functionApi). then (response => {
        return response.json();
    }). then (respone => {
         console.log("website called function api");
            count = respone.count;
            document.getElementById("counter").innerText = count;
    }).catch(function(error){
        console.log(error);

    }}
    return count;
}    