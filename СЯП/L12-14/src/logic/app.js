import { Sudoku } from '../entities/Sudoku.js';
import { createBoard, renderBoard, clearHighlights, highlightErrors, highlightSuccess } from '../draw/ui.js';

let sudoku;
let sudokuContainer;

function handleNewGame() {
    sudoku.generateNewPuzzle(40);
    renderBoard(sudoku.board);
}

function handleCheck() {
    if (sudoku.isFullySolved()) {
        highlightSuccess();
    } else {
        const errors = sudoku.checkAll();
        if (errors.invalidRows.length === 0 && errors.invalidCols.length === 0 && errors.invalidSquares.length === 0) {
            clearHighlights();
        } else {
            highlightErrors(errors);
        }
    }
}

function handleSolve() {
    sudoku.generateSolvedBoard();
    renderBoard(sudoku.board);
}

function handleCellUpdate(row, col, value) {
    sudoku.updateBoard(row, col, value);
}

export function init() {
    sudoku = new Sudoku();
    sudokuContainer = document.getElementById('sudoku-container');
    
    const newGameBtn = document.getElementById('new-game-btn');
    const checkBtn = document.getElementById('check-btn');
    const solveBtn = document.getElementById('solve-btn');

    newGameBtn.addEventListener('click', handleNewGame);
    checkBtn.addEventListener('click', handleCheck);
    solveBtn.addEventListener('click', handleSolve);
    
    createBoard(sudokuContainer, handleCellUpdate);
    handleNewGame();
}
