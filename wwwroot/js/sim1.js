const canvas = document.getElementById("simCanvas");
const ctx = canvas.getContext("2d");
function randInt(min, max) {
    min = Math.ceil(min);
    max = Math.floor(max);
    return Math.floor(Math.random() * (max - min + 1)) + min;
}
const planet = {
    x: randInt(0, 1920),
    y: randInt(0, 1080),
    radius: 50,
    function draw() {
        ctx.fillStyle = color(1); // Set your color first
        ctx.beginPath();
        ctx.arc(x, y, radius, 0, Math.PI * 2);
        ctx.fill();
    }
};
const planets = [];
function addPlanet() {
    planets.push({});
    planets[0].draw();
}


/*function color(int count) {
    switch (count) {
        case 0:
            return "blue";
            break;
        case 1:
            return "red";
            break;
        case 2:
            return "green";
            break;
    }
}*/