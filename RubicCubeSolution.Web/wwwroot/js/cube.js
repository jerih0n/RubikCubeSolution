// Color mapping for Rubik's Cube cells
const colorMap = {
    0: 'transparent',  // None
    1: '#FF6B35',      // Orange
    2: '#FFFFFF',      // White
    3: '#00A651',      // Green
    4: '#FFD700',      // Yellow
    5: '#DC143C',      // Red
    6: '#0066CC'       // Blue
};

// Color name mapping
const colorNames = {
    0: 'None',
    1: 'Orange',
    2: 'White',
    3: 'Green',
    4: 'Yellow',
    5: 'Red',
    6: 'Blue'
};

// Button press history array
let buttonPressHistory = [];

// Initialize cube functionality when DOM is ready
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
            const instruction = this.getAttribute('data-instruction');
            addToHistory(instruction);
            performRotation(instruction);
        });
    });
}

/**
 * Initialize reset button event listener
 */
function initializeResetButton() {
    const resetButton = document.getElementById('resetBtn');
    if (resetButton) {
        resetButton.addEventListener('click', function() {
            resetCube();
        });
    }
}

/**
 * Add instruction to history
 * @param {string} instruction - The rotation instruction (e.g., 'F', 'R', 'U', etc.)
 */
function addToHistory(instruction) {
    buttonPressHistory.push(instruction);
    updateHistoryDisplay();
}

/**
 * Update the history display element
 */
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

/**
 * Perform a rotation on the cube
 * @param {string} instruction - The rotation instruction
 */
function performRotation(instruction) {
    // Disable all buttons during rotation
    const allButtons = document.querySelectorAll('.rotation-btn, .reset-btn');
    allButtons.forEach(btn => btn.disabled = true);

    fetch('/Home/Rotate', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify({ instruction: instruction })
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
        // Re-enable all buttons
        allButtons.forEach(btn => btn.disabled = false);
    });
}

/**
 * Update the matrix display with new data
 * @param {Array<Array<number>>} matrix - The matrix data from the server
 */
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

/**
 * Get color name from enum value
 * @param {number} value - The color enum value
 * @returns {string} The color name
 */
function getColorName(value) {
    return colorNames[value] || 'Unknown';
}

/**
 * Reset the cube to its initial state
 */
function resetCube() {
    // Disable buttons during reset
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
        // Clear button press history
        buttonPressHistory = [];
        updateHistoryDisplay();
    })
    .catch(error => {
        console.error('Error:', error);
        alert('Error resetting cube: ' + error.message);
    })
    .finally(() => {
        // Re-enable buttons
        const buttons = document.querySelectorAll('.rotation-btn, .reset-btn');
        buttons.forEach(btn => btn.disabled = false);
    });
}
