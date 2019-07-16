"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
const http = require("http");
const mediaServer_1 = require("./mediaServer");
var port = process.env.port || 1337;
http.createServer(function (req, res) {
    res.writeHead(200, { 'Content-Type': 'text/plain' });
    res.end('Hello World\n');
}).listen(port);
mediaServer_1.StartMediaServer();
//# sourceMappingURL=server.js.map