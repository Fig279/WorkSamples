//#region BoardSpace Constructor

//each space that is occupied by a token
class boardSpace {

    constructor(downDiagonal, rightDiagonal, column, row) {
        this.token = "nothing";
        this.column = column;
        this.row = row;
        this.downDiagonal = downDiagonal;
        this.rightDiagonal = rightDiagonal;


        //create the board space in HTML
        let space = document.createElement("span");
        space.innerHTML = "<img src='images/nothing.png' alt='token space'>"
        space.className = "cell";
        space.style.left = `${column * (6.5)}vw`;
        space.style.top = `${row * (6.5)}vw`;
        this.tokenSpace = space;

        this.tokenSpace.onclick = placeToken;
        boardContainer.appendChild(this.tokenSpace);
    }
}

class previousBoardSpace {
    constructor() {
        this.token = "nothing";
        this.img = "<img src='images/nothing.png' alt='token space'>"
    }
}
//#endregion

//#region Player Displayed Info
let activePlayerDisplayer = document.querySelector("#activePlayer");
let whiteTokenCount = document.querySelector("#whiteTokenCount");
let blackTokenCount = document.querySelector("#blackTokenCount");
let tokensLastTurn = document.querySelector("#tokensTaken");
let turnNumber = document.querySelector("#turnNumber");


let infoOnRight = document.querySelector("#info")
let winner = document.createElement("p");

const soundPlayer = new Audio("audio/token_placing.mp3");
const winSound = new Audio("audio/win_sound.wav")

//updates the players displayed info according to what happened in a turn
//forwards will = 1 if the turn is moving forwards and -1 if it is moving backwards
function updateInfo(forwards = 1) {

    activePlayerDisplayer.innerHTML = activePlayer + `<img src="images/${activePlayer}_token.png">`;

    let blackTokens = 0;
    let whiteTokens = 0;
    for (let r = 0; r < 8; r++) {
        for (let c = 0; c < 8; c++) {
            if (board[r][c].token === "white") {
                whiteTokens++;
            }
            else if (board[r][c].token === "black") {
                blackTokens++;
            }
        }
    }

    whiteTokenCount.innerHTML = whiteTokens;
    blackTokenCount.innerHTML = blackTokens;

    tokensLastTurn.innerHTML = tokensFlipped * forwards;
    turnNumber.innerHTML = Number(turnNumber.innerHTML) + 1 * forwards;

    if (blackTokens + whiteTokens === 64) {
        //end game
        winSound.play();
        if (blackTokens > whiteTokens) {
            winner.innerHTML = "Black Player Wins!"
        }
        else {
            winner.innerHTML = "White Player Wins!"

        }
        infoOnRight.appendChild(winner);
    }
}

//#endregion

//#region Create the Board

let tokensFlipped = 0;

//holds all spaces
let board = [[], [], [], [], [], [], [], []];
const boardContainer = document.querySelector("#gridContainer");

//alternates between white and black player
let activePlayer = "white";
let inactivePlayer = "black";

//initializes the board with correct diagonal values
for (let r = 0; r < 8; r++) {
    for (let c = 0; c < 8; c++) {
        board[r][c] = new boardSpace(c - r, c + r, c, r);
    }
}

let previousBoard = [[], [], [], [], [], [], [], []];

startNewGame();

//sets the previous board to the default values
function setPreviousBoard() {
    //creates the previous board
    for (let r = 0; r < 8; r++) {
        for (let c = 0; c < 8; c++) {
            previousBoard[r][c] = new previousBoardSpace();
            previousBoard[r][c].img = board[r][c].tokenSpace.innerHTML;
            previousBoard[r][c].token = board[r][c].token;
        }

    }
}

//sets all values for boardspaces to their default starting values
//updates player information accordingly
function startNewGame() {
    //clear the board
    for (let r = 0; r < 8; r++) {
        for (let c = 0; c < 8; c++) {
            board[r][c].tokenSpace.innerHTML = "<img src='images/nothing.png' alt='token space'>";
            board[r][c].token = "nothing";
        }
    }

    //starting tiles
    board[3][3].token = "white";
    board[3][3].tokenSpace.innerHTML = "<img src='images/white_token.png' alt='token space'>";

    board[3][4].token = "black";
    board[3][4].tokenSpace.innerHTML = "<img src='images/black_token.png' alt='token space'>";

    board[4][3].token = "black";
    board[4][3].tokenSpace.innerHTML = "<img src='images/black_token.png' alt='token space'>";

    board[4][4].token = "white";
    board[4][4].tokenSpace.innerHTML = "<img src='images/white_token.png' alt='token space'>";

    tokensFlipped = 0;

    setPreviousBoard();

    activePlayer = "black";
    inactivePlayer = "white";

    turnNumber.innerHTML = 0;
    updateInfo();

    winner.innerHTML = "";
}

//#endregion

//#region Gameplay Logic

//called when a player places a token
//checks if that token will flip any tokens, if so it is played and the tokens are flipped
//if not then the token is not allowed to be placed
function placeToken() {
    //console.log("place token");
    row = getSpanSpot(this, "row");
    column = getSpanSpot(this, "column");

    //if this space can be claimed
    if (board[row][column].token === "nothing") {


        //tracking previous board
        updatePreviousBoard();

        //change space token
        board[row][column].token = activePlayer;

        //flip other required tiles
        checkOutflanking(board[row][column]);

        if (tokensFlipped >= 1) {

            soundPlayer.play();

            //switches the players turn over
            if (activePlayer === "white") {
                activePlayer = "black"
                inactivePlayer = "white";
            }
            else if (activePlayer === "black") {
                activePlayer = "white"
                inactivePlayer = "black";
            }

            updateInfo(1);
        }
        else {

            //gray out undo button
            undoButton.style.backgroundColor = "#919090";

            //can't play there
            board[row][column].token = "nothing";
            board[row][column].tokenSpace.innerHTML = "<img src='images/nothing.png' alt='token space'>";
        }
    }
}

//find the row of the corresponding span 
//- helper function to find row and column
function getSpanSpot(span, area) {
    let value;

    // remove units 
    if (area === "row") {
        value = Number(span.style.top.substring(0, span.style.top.length - 2));

        //divide to find row
        value = value / 6.5;

        //if there's an error
        if (value % 1 != 0) {
            value += 1 - 0.923076923076923;
        }
    }
    else if (area === "column") {
        value = Number(span.style.left.substring(0, span.style.left.length - 2));
        //divide to find column
        value = value / 6.5;
    }
    return value;
}

//calls each direction of checking
function checkOutflanking(boardSpace) {

    tokensFlipped = 0;

    checkHorizantally(boardSpace);
    checkVertically(boardSpace);
    checkDiagonally(boardSpace);

}

//#region Check Flips

//checks all tokens in placed tokens row to see if they need to be flipped
//if so, flips them
//first going right, then left
function checkHorizantally(boardSpace) {
    let nextTile = null;
    let directionToMove = 1;

    do {
        let selectedTileColumn = boardSpace.column;

        //horizontal checking to the right of space
        do {
            //tile to the right
            selectedTileColumn += directionToMove;

            //if there is a column there
            if (selectedTileColumn < 8 && selectedTileColumn > -1) {
                nextTile = board[boardSpace.row][selectedTileColumn];
            }
            else {
                nextTile = boardSpace;
            }

        } while (nextTile.token == inactivePlayer);

        //if the correct color token is on the other end
        if (nextTile.token == boardSpace.token && nextTile != boardSpace) {

            tokensFlipped--;

            //flip all the previously checked tokens until getting back to placed token
            do {
                tokensFlipped++;

                selectedTileColumn -= directionToMove;
                nextTile = board[boardSpace.row][selectedTileColumn];

                nextTile.token = boardSpace.token;
                nextTile.tokenSpace.innerHTML = `<img src='images/${boardSpace.token}_token.png' alt='token space'>`
            } while (nextTile != boardSpace);
        }

        //change directions
        directionToMove -= 2;
    } while (directionToMove >= -1)

}

//checks all tokens in placed tokens column to see if they need to be flipped
//if so, flips them
//first going down, then up
function checkVertically(boardSpace) {
    let nextTile = null;
    let directionToMove = 1;

    do {
        let selectedTileRow = boardSpace.row;

        //Vertical checking going down
        do {
            //tile going down the first time, up the second
            selectedTileRow += directionToMove;

            //if there is another tile
            if (selectedTileRow < 8 && selectedTileRow > -1) {
                nextTile = board[selectedTileRow][boardSpace.column];
            }
            else {
                nextTile = boardSpace;
            }

        } while (nextTile.token == inactivePlayer);

        //if the correct color token is on the other end
        if (nextTile.token == boardSpace.token && nextTile != boardSpace) {
            tokensFlipped--;

            //flip all the previously checked tokens until getting back to placed token
            do {
                tokensFlipped++;
                selectedTileRow -= directionToMove;
                nextTile = board[selectedTileRow][boardSpace.column];
                nextTile.token = boardSpace.token;
                nextTile.tokenSpace.innerHTML = `<img src='images/${boardSpace.token}_token.png' alt='token space'>`
            } while (nextTile != boardSpace);
        }

        //change directions
        directionToMove -= 2;
    } while (directionToMove >= -1);
}

//iterate through tiles diagonally from placed token to see if they should flip
function checkDiagonally(boardSpace) {
    let nextTile = null;
    let directionToMoveRow = 1;
    let directionsToMoveColumn = 1;

    do {

        selectedTileColumn = boardSpace.column;
        selectedTileRow = boardSpace.row;

        //iterate through tiles diagonally
        do {
            selectedTileColumn += directionsToMoveColumn;
            selectedTileRow += directionToMoveRow;

            //if there is a tile there
            if (selectedTileColumn < 8 && selectedTileColumn > -1
                && selectedTileRow < 8 && selectedTileRow > -1) {

                //check this tile
                nextTile = board[selectedTileRow][selectedTileColumn];
            }
            else {
                //no more board, end iteration
                nextTile = boardSpace;
            }

        } while (nextTile.token == inactivePlayer);

        //if the correct color token is on the other end
        if (nextTile.token == boardSpace.token && nextTile != boardSpace) {

            tokensFlipped--;
            //flip all the previously checked tokens until getting back to placed token
            do {
                tokensFlipped++;

                selectedTileColumn -= directionsToMoveColumn;
                selectedTileRow -= directionToMoveRow;

                nextTile = board[selectedTileRow][selectedTileColumn];
                nextTile.token = boardSpace.token;
                nextTile.tokenSpace.innerHTML = `<img src='images/${boardSpace.token}_token.png' alt='token space'>`;
            } while (nextTile != boardSpace);
        }

        //first - down and right
        //change directions
        if (directionToMoveRow === 1 && directionsToMoveColumn === 1) {
            //go down and left
            directionsToMoveColumn = -1;
        }
        else if (directionToMoveRow === 1 && directionsToMoveColumn === -1) {
            //go up and left
            directionToMoveRow = -1;
        }
        else if (directionToMoveRow === -1 && directionsToMoveColumn === -1) {
            //up and right
            directionsToMoveColumn = 1;
        }
        else {
            //end
            directionToMoveRow = -3;
        }

    } while (directionToMoveRow > -3);

}
//#endregion

//#endregion

//#region Additional Player Buttons
const passTurn = document.querySelector("#pass");
passTurn.addEventListener("click", passPlayerTurn);

//pressing undo button undoes previous turn
const undoButton = document.querySelector("#undo");
undoButton.onclick = undoMove;

//reset board to default
const newGameButton = document.querySelector("#newGame");
newGameButton.onclick = startNewGame;

//switches the active and inactive player to move on to the inactive player's turn
function passPlayerTurn(forwards = 1) {

    if (forwards != 1 && forwards != -1) {
        forwards = 1;
    }

    if (activePlayer === "black") {
        activePlayer = "white";
        inactivePlayer = "black";
        blackTokenCount.innerHTML = Number(blackTokenCount.innerHTML) - 1;

    }
    else {
        activePlayer = "black";
        inactivePlayer = "white";
        whiteTokenCount.innerHTML = Number(whiteTokenCount.innerHTML) - 1;

    }

    tokensFlipped = 0;

    //gray out button as it can't be used
    undoButton.style.backgroundColor = "#919090";

    //adjust displayed tokens and player
    updateInfo(forwards);

}

//returns the board to the state it was in before the last move
//undo can only be used once
function undoMove() {
    //console.log("undo");

    //exit early as there is no information about previous turn
    if (tokensFlipped === 0) {
        return;
    }

    //restores the previous board
    for (let r = 0; r < 8; r++) {
        for (let c = 0; c < 8; c++) {
            //board[r][c] = new boardSpace(
            //  previousBoard[r][c].downDiagonal, previousBoard[r][c].rightDiagonal,
            //previousBoard[r][c].column, previousBoard[r][c].row);

            board[r][c].tokenSpace.innerHTML = previousBoard[r][c].img;
            board[r][c].token = previousBoard[r][c].token;
        }
    }

    //move turn back to previous player
    passPlayerTurn(-1);
}

//sets the previous board values to what the board currently is as it is about to change
function updatePreviousBoard() {
    for (let r = 0; r < 8; r++) {
        for (let c = 0; c < 8; c++) {
            previousBoard[r][c].img = board[r][c].tokenSpace.innerHTML;
            previousBoard[r][c].token = board[r][c].token;
        }
    }
    undoButton.style.backgroundColor = "rgb(229, 229, 229)";
}

//#endregion