let SERVER_PORT = null;

const serverStartButton = document.getElementById("start-server");
serverStartButton.addEventListener("click", startServer, true);

async function startServer() {
    const serverPortInput = document.getElementById("server-port");

    const reqParams = {
        method: "POST",
        mode: "cors",
        headers: {
            "Content-Type": "application/json"
        },
    }

    reqParams.body = serverPortInput.value;
    SERVER_PORT = serverPortInput.value;

    let res = await fetch('http://localhost:5012/Server/start', reqParams);
    let body = await res.json();

    displayServerStatus();
}

async function displayServerStatus() {
    const serverStatusDisplay = document.getElementById("server-status-display");

    const reqParams = {
        method: "GET",
        mode: "cors",
        headers: {
            "Content-Type": "application/json"
        },
    }

    let res = await fetch('http://localhost:5012/Server/status', reqParams);
    let body = await res.json();

    SERVER_PORT = body.port;
    serverStatusDisplay.textContent = body.status ? `server is listening on port ${SERVER_PORT}` : `server is down`;
}

const getServerInfoButton = document.getElementById("get-server-info");
getServerInfoButton.addEventListener("click", getServerInfo, true);

async function getServerInfo() {
    if (!SERVER_PORT)
        return;

    const reqParams = {
        method: "GET",
        mode: "cors",
        headers: {
            "Content-Type": "application/json"
        },
    }

    let res = await fetch(`http://localhost:${SERVER_PORT}/`, reqParams);
    let body = await res.json();

    console.log(body);

    let serverInfo = document.getElementById("server-info");
    let getInfo = body.graphInfo.GET;
    let postInfo = body.graphInfo.POST;
    let overallRequestAmount = getInfo.amount + postInfo.amount;
    let overallRequestTime = getInfo.overallTime + postInfo.overallTime;
    let serverInfoString = `uptime: ${body.uptime}\noverall requests: ${overallRequestsAmount}\n\tGET: ${getInfo.amount}\n\tPOST: ${postInfo.amount}\navarage time: ${overallRequestTime / overallRequestAmount}`;
    serverInfo.textContent = serverInfoString;
}

function main() {
    displayServerStatus();
}

main();



/*
const sendRequestButton = document.getElementById("send-request");
sendRequestButton.addEventListener("click", sendRequest, true);

async function sendRequest() {
    let method = document.getElementById("request-method").value;
    let uri = document.getElementById("request-uri").value;

    let localReqParams = {
        method: method,
        mode: 'cors',
    }

    let res = await fetch(uri, localReqParams);
    let body = await res.json();

    console.log(body);
}
*/