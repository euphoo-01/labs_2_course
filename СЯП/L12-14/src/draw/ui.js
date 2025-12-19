export function createBoard(container, onCellUpdate) {
    container.innerHTML = '';
    for (let i = 0; i < 81; i++) {
        const cell = document.createElement('input');
        cell.type = 'text';
        cell.maxLength = 1;
        cell.classList.add('sudoku-cell');
        const row = Math.floor(i / 9);
        const col = i % 9;
        cell.dataset.row = row;
        cell.dataset.col = col;

        if ((col + 1) % 3 === 0 && col < 8) {
            cell.classList.add('border-right');
        }
        if ((row + 1) % 3 === 0 && row < 8) {
            cell.classList.add('border-bottom');
        }
        
        cell.addEventListener('input', (e) => {
            const value = e.target.value.replace(/[^1-9]/g, '');
            e.target.value = value;
            onCellUpdate(row, col, value === '' ? 0 : parseInt(value, 10));
        });
        
        container.appendChild(cell);
    }
}

export function renderBoard(board) {
    const cells = document.querySelectorAll('.sudoku-cell');
    cells.forEach(cell => {
        const row = cell.dataset.row;
        const col = cell.dataset.col;
        const value = board[row][col];
        if (value !== 0) {
            cell.value = value;
            cell.readOnly = true;
            cell.style.backgroundColor = '#eee';
        } else {
            cell.value = '';
            cell.readOnly = false;
            cell.style.backgroundColor = 'white';
        }
    });
}

export function clearHighlights() {
    const cells = document.querySelectorAll('.sudoku-cell');
    cells.forEach(cell => {
        cell.style.backgroundColor = cell.readOnly ? '#eee' : 'white';
        cell.style.color = 'black';
    });
     document.getElementById('sudoku-container').style.backgroundColor = 'transparent';
}

export function highlightErrors({ invalidRows, invalidCols, invalidSquares }) {
    clearHighlights();
    const cells = document.querySelectorAll('.sudoku-cell');
    
    cells.forEach(cell => {
        const row = parseInt(cell.dataset.row);
        const col = parseInt(cell.dataset.col);
        const square = Math.floor(row / 3) * 3 + Math.floor(col / 3);
        
        if (invalidRows.includes(row) || invalidCols.includes(col) || invalidSquares.includes(square)) {
            cell.style.backgroundColor = '#ffdddd';
        }
    });
}

export function highlightSuccess() {
    clearHighlights();
    document.getElementById('sudoku-container').style.backgroundColor = '#ffffcc';
}
