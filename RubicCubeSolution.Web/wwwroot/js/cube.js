const colorMap = {
    0: 'transparent',
    1: '#FF6B35',
    2: '#FFFFFF',
    3: '#00A651',
    4: '#FFD700',
    5: '#DC143C',
    6: '#0066CC'
};

const colorNames = {
    0: 'None',
    1: 'Orange',
    2: 'White',
    3: 'Green',
    4: 'Yellow',
    5: 'Red',
    6: 'Blue'
};

let buttonPressHistory = [];

document.addEventListener('DOMContentLoaded', function() {
    initializeRotationButtons();
    initializeResetButton();
    updateHistoryDisplay();
});

/**
 * Initialize rotation button event listeners
 */
function initializeRotationButtons() {
    const rotationButtons = document.querySelectorAll('.rotation-btn');
    
    rotationButtons.forEach(button => {
        button.addEventListener('click', function() {
            const side = parseInt(this.getAttribute('data-side'));
            const clockwise = this.getAttribute('data-clockwise') === 'true';
            const label = this.getAttribute('data-label');
            addToHistory(label);
            performRotation(side, clockwise);
        });
    });
}

function initializeResetButton() {
    const resetButton = document.getElementById('resetBtn');
    if (resetButton) {
        resetButton.addEventListener('click', function() {
            resetCube();
        });
    }
}

function addToHistory(label) {
    buttonPressHistory.push(label);
    updateHistoryDisplay();
}

function updateHistoryDisplay() {
    const historyList = document.getElementById('historyList');
    
    if (!historyList) {
        return;
    }
    
    if (buttonPressHistory.length === 0) {
        historyList.innerHTML = '<p class="text-muted text-center">No rotations yet</p>';
    } else {
        let html = '';
        // Show history in chronological order (oldest to newest)
        for (let i = 0; i < buttonPressHistory.length; i++) {
            html += `<div class="history-item">${buttonPressHistory[i]}</div>`;
        }
        historyList.innerHTML = html;
        // Scroll to the end to show most recent items
        historyList.scrollLeft = historyList.scrollWidth;
    }
}

function performRotation(side, clockwise) {
    const allButtons = document.querySelectorAll('.rotation-btn, .reset-btn');
    allButtons.forEach(btn => btn.disabled = true);

    fetch('/Home/Rotate', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify({ side: side, clockwise: clockwise })
    })
    .then(response => {
        if (!response.ok) {
            return response.json().then(err => {
                throw new Error(err.error || 'Network response was not ok');
            });
        }
        return response.json();
    })
    .then(data => {
        if (data.matrix) {
            updateMatrix(data.matrix);
        } else {
            throw new Error('Invalid response format');
        }
    })
    .catch(error => {
        console.error('Error:', error);
        alert('Error performing rotation: ' + error.message);
    })
    .finally(() => {
        allButtons.forEach(btn => btn.disabled = false);
    });
}

function updateMatrix(matrix) {
    const container = document.getElementById('matrixContainer');
    if (!container) {
        return;
    }
    
    container.innerHTML = '';

    for (let i = 0; i < matrix.length; i++) {
        const row = document.createElement('div');
        row.className = 'matrix-row';

        for (let j = 0; j < matrix[i].length; j++) {
            const cellValue = matrix[i][j];
            
            if (cellValue !== 0) { // Not None
                const cell = document.createElement('div');
                cell.className = 'matrix-cell';
                cell.style.backgroundColor = colorMap[cellValue];
                cell.title = getColorName(cellValue) + ' (' + i + ', ' + j + ')';
                row.appendChild(cell);
            } else {
                const emptyCell = document.createElement('div');
                emptyCell.className = 'matrix-cell-empty';
                row.appendChild(emptyCell);
            }
        }

        container.appendChild(row);
    }
}

function getColorName(value) {
    return colorNames[value] || 'Unknown';
}

function resetCube() {
    const buttons = document.querySelectorAll('.rotation-btn, .reset-btn');
    buttons.forEach(btn => btn.disabled = true);

    fetch('/Home/Reset', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        }
    })
    .then(response => {
        if (!response.ok) {
            throw new Error('Network response was not ok');
        }
        return response.json();
    })
    .then(data => {
        updateMatrix(data.matrix);
        buttonPressHistory = [];
        updateHistoryDisplay();
    })
    .catch(error => {
        console.error('Error:', error);
        alert('Error resetting cube: ' + error.message);
    })
    .finally(() => {
        const buttons = document.querySelectorAll('.rotation-btn, .reset-btn');
        buttons.forEach(btn => btn.disabled = false);
    });
}
