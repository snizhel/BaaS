const config = { databaseURL: "https://serviceemploy-5f3f5-default-rtdb.asia-southeast1.firebasedatabase.app/"}; 
firebase.initializeApp (config); 
const dbRef = firebase.database().ref(); 



function page_Load(){
    laydanhsach();
}

function btnSearch_Click(){
    var keyword = document.getElementById("txtKeyword").value.trim();
    if(keyword.length > 0){
        search(keyword);
    }else{
        laydanhsach();
    }
}

function lnkID_Click(Manv){
    getDetails(Manv);
}

function btnAdd_Click(){
    var newnhanvien = {
        ID: document.getElementById("txtManv").value,
        Name: document.getElementById("txtTen").value,
        Address: document.getElementById("txtChucvu").value,
        Salary: document.getElementById("txtPhongban").value,
        Age: document.getElementById("txtChuthich").value
    };
    addNew(newnhanvien);
}

function btnUpdate_Click(){
    var newnhanvien = {
        ID:document.getElementById("txtManv").value,
        Name: document.getElementById("txtTen").value,
        Address: document.getElementById("txtChucvu").value,
        Salary: document.getElementById("txtPhongban").value,
        Age: document.getElementById("txtChuthich").value
    };
    update(newnhanvien);
}

function btnDelete_Click(){
    if(confirm("ARE U SURE?")){
        var manv = document.getElementById("txtManv").value;
        deletee(manv);
    }
}

function laydanhsach() { 
    dbRef.child("nhanviens").on("value", (snapshot) => { 
        var nhanviens = [];
        snapshot.forEach((child) => { 
        //alert(child.key); 
        var nhanvien = child.val(); 
        nhanviens.push (nhanvien); }); 
        renderNVList (nhanviens); });     
}

function getDetails(manv){
    dbRef.child("nhanviens").once ("value", (snapshot) => { 
        snapshot.forEach((child) => { 
        var nhanvien = child.val();
         if (nhanvien.ID == manv) { 
        renderNVDetails (nhanvien); 
        }
    }); 
    }); 
        
}

function search(keyword){
    dbRef.child("nhanviens").once ("value", (snapshot) => { 
        var nhanviens = []; snapshot.forEach((child) => { 
        var nhanvien = child.val(); if (nhanvien.Name.toLowerCase().includes (keyword.toLowerCase())) { 
        nhanviens.push (nhanvien);}
        }); 
        renderNVList (nhanviens); });
}

function addNew(newnhanvien){
    dbRef.child("nhanviens/NV" + newnhanvien.ID).set(newnhanvien); 
}

function update(newnhanvien){
    dbRef.child("nhanviens").once ("value", (snapshot) => { 
        snapshot.forEach((child) => { 
        var nhanvien = child.val(); 
        if (nhanvien.ID == newnhanvien.ID) { 
        var key = child. key; 
        dbRef.child("nhanviens").child(key).set(newnhanvien); 
    }
        }); }); 
        
}

function deletee(manv){
    dbRef.child("nhanviens").once ("value", (snapshot) => { 
        snapshot.forEach((child) => { 
        var nhanvien = child.val(); if (nhanvien.ID == manv) { 
        var key = child. key; dbRef.child ("nhanviens").child (key).remove();} 
        }); 
        }); 
        
}

function renderNVList(nhanviens){
    var rows = "";
    for(var ChitietNV of nhanviens){
        rows += "<tr onclick='lnkID_Click(" + ChitietNV.ID + ")' style='cursor:pointer'>";
        rows += "<td>" + ChitietNV.ID + "</td>";
        rows += "<td>" + ChitietNV.Name + "</td>";
        rows += "<td>" + ChitietNV.Address + "</td>";
        rows += "<td>" + ChitietNV.Salary + "</td>";
        rows += "<td>" + ChitietNV.Age + "</td>";
        rows += "</td>";
    }
    var header = "<tr><th>Manv</th><th>Ten</th><th>Chucvu</th><th>Phongban</th><th>Chuthich</th></tr>";
    document.getElementById("lstNVS").innerHTML = header + rows;
}

function renderNVDetails(nhanvien){
    document.getElementById("txtManv").value = nhanvien.ID;
    document.getElementById("txtTen").value = nhanvien.Name ;
    document.getElementById("txtChucvu").value = nhanvien.Address;
    document.getElementById("txtPhongban").value = nhanvien.Salary;
    document.getElementById("txtChuthich").value = nhanvien.Age;
}

function clearTextboxes(){
    document.getElementById("txtManv").value ='';
    document.getElementById("txtTen").value ='';
    document.getElementById("txtChucvu").value ='';
    document.getElementById("txtPhongban").value ='';
    document.getElementById("txtChuthich").value ='';
}

