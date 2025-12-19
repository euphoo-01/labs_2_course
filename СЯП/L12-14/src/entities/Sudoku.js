export class Sudoku {
    constructor() {
        this.board = this.createEmptyBoard();
        this.initialBoard = this.createEmptyBoard();
    }

    createEmptyBoard() {
        return Array(9).fill(null).map(() => Array(9).fill(0));
    }

    generateSolvedBoard() {
        this.board = this.createEmptyBoard();
        this.solve();
        this.initialBoard = JSON.parse(JSON.stringify(this.board));
    }
    
    generateNewPuzzle(difficulty = 40) {
        this.generateSolvedBoard();
        let cellsToRemove = difficulty;
        while (cellsToRemove > 0) {
            const row = Math.floor(Math.random() * 9);
            const col = Math.floor(Math.random() * 9);
            if (this.board[row][col] !== 0) {
                this.board[row][col] = 0;
                cellsToRemove--;
            }
        }
        this.initialBoard = JSON.parse(JSON.stringify(this.board));
    }


    solve() {
        const find = this.findEmpty();
        if (!find) {
            return true;
        }
        const [row, col] = find;

        const numbers = [1, 2, 3, 4, 5, 6, 7, 8, 9].sort(() => Math.random() - 0.5);

        for (let num of numbers) {
            if (this.isValid(num, row, col)) {
                this.board[row][col] = num;

                if (this.solve()) {
                    return true;
                }

                this.board[row][col] = 0;
            }
        }
        return false;
    }
    
    isFullySolved() {
    for (let i = 0; i < 9; i++) {
        for (let j = 0; j < 9; j++) {
            if (this.board[i][j] === 0) {
                return false;
            }
        }
    }
    const errors = this.checkAll();
    return errors.invalidRows.length === 0 && errors.invalidCols.length === 0 && errors.invalidSquares.length === 0;
}


    isValid(num, row, col) {
        for (let i = 0; i < 9; i++) {
            if (this.board[row][i] === num && col !== i) {
                return false;
            }
        }
        for (let i = 0; i < 9; i++) {
            if (this.board[i][col] === num && row !== i) {
                return false;
            }
        }
        const boxX = Math.floor(col / 3);
        const boxY = Math.floor(row / 3);
        for (let i = boxY * 3; i < boxY * 3 + 3; i++) {
            for (let j = boxX * 3; j < boxX * 3 + 3; j++) {
                if (this.board[i][j] === num && (i !== row || j !== col)) {
                    return false;
                }
            }
        }
        return true;
    }

    findEmpty() {
        for (let i = 0; i < 9; i++) {
            for (let j = 0; j < 9; j++) {
                if (this.board[i][j] === 0) {
                    return [i, j];
                }
            }
        }
        return null;
    }

    checkAll() {
        const invalidRows = this.checkRows();
        const invalidCols = this.checkCols();
        const invalidSquares = this.checkSquares();
        
        console.log('Invalid Rows:', invalidRows);
        console.log('Invalid Cols:', invalidCols);
        console.log('Invalid Squares:', invalidSquares);

        return { invalidRows, invalidCols, invalidSquares };
    }

    checkRows() {
        const invalid = [];
        for (let i = 0; i < 9; i++) {
            if (this.hasDuplicates(this.board[i])) {
                invalid.push(i);
            }
        }
        return invalid;
    }

    checkCols() {
        const invalid = [];
        for (let j = 0; j < 9; j++) {
            const col = [];
            for (let i = 0; i < 9; i++) {
                col.push(this.board[i][j]);
            }
            if (this.hasDuplicates(col)) {
                invalid.push(j);
            }
        }
        return invalid;
    }

    checkSquares() {
        const invalid = [];
        for (let i = 0; i < 9; i++) {
            const square = [];
            const startRow = Math.floor(i / 3) * 3;
            const startCol = (i % 3) * 3;
            for (let r = 0; r < 3; r++) {
                for (let c = 0; c < 3; c++) {
                    square.push(this.board[startRow + r][startCol + c]);
                }
            }
            if (this.hasDuplicates(square)) {
                invalid.push(i);
            }
        }
        return invalid;
    }

    hasDuplicates(arr) {
        const filtered = arr.filter(num => num !== 0);
        return new Set(filtered).size !== filtered.length;
    }
    
    updateBoard(row, col, value) {
        if (this.initialBoard[row][col] === 0) {
            this.board[row][col] = value;
            return true;
        }
        return false;
    }

    reset() {
        this.board = JSON.parse(JSON.stringify(this.initialBoard));
    }
}
