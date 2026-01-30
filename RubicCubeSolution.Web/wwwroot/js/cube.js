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
    initializeTestSequenceButton();
    updateHistoryDisplay();
});

/**
 * Initialize rotation button event listeners
 */
function initializeRotationButtons() {
    const rotationButtons = document.querySelectorAll('.rotation-btn');
    
    rotationButtons.forEach(button => {
        button.addEventListener('click', async function() {
            const side = parseInt(this.getAttribute('data-side'));
            const clockwise = this.getAttribute('data-clockwise') === 'true';
            const label = this.getAttribute('data-label');
            addToHistory(label);
            await performRotation(side, clockwise);
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

function initializeTestSequenceButton() {
    const testSequenceButton = document.getElementById('testSequenceBtn');
    if (testSequenceButton) {
        testSequenceButton.addEventListener('click', function() {
            runTestSequence();
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

/**
 * Perform rotation and return a Promise
 * @param {number} side - The side to rotate
 * @param {boolean} clockwise - Whether to rotate clockwise
 * @param {boolean} showAlert - Whether to show alert on error (default: true)
 * @returns {Promise} Promise that resolves with the rotation data
 */
async function performRotation(side, clockwise, showAlert = true) {
    const allButtons = document.querySelectorAll('.rotation-btn, .reset-btn, .test-sequence-btn');
    allButtons.forEach(btn => btn.disabled = true);

    try {
        const response = await fetch('/Home/Rotate', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({ side: side, clockwise: clockwise })
        });

        if (!response.ok) {
            const err = await response.json();
            throw new Error(err.error || 'Network response was not ok');
        }

        const data = await response.json();
        
        if (data.matrix) {
            updateMatrix(data.matrix);
            return data;
        } else {
            throw new Error('Invalid response format');
        }
    } catch (error) {
        console.error('Error:', error);
        if (showAlert) {
            alert('Error performing rotation: ' + error.message);
        }
        throw error; // Re-throw so caller can handle it
    } finally {
        allButtons.forEach(btn => btn.disabled = false);
    }
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
    const buttons = document.querySelectorAll('.rotation-btn, .reset-btn, .test-sequence-btn');
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
        const buttons = document.querySelectorAll('.rotation-btn, .reset-btn, .test-sequence-btn');
        buttons.forEach(btn => btn.disabled = false);
    });
}

/**
 * Run test sequence: F (CW), R (CCW), U (CW), B (CCW), L (CW), D (CCW)
 */
async function runTestSequence() {
    // Define the test sequence
    // RubikCubeSideEnum: Front = 3, Right = 5, Upper = 2, Bottom = 6, Left = 1, Down = 4
    const sequence = [
        { side: 3, clockwise: true, label: 'F' },      // Front CW
        { side: 5, clockwise: false, label: 'R\'' },  // Right CCW
        { side: 2, clockwise: true, label: 'U' },      // Upper CW
        { side: 6, clockwise: false, label: 'B\'' },   // Bottom CCW
        { side: 1, clockwise: true, label: 'L' },      // Left CW
        { side: 4, clockwise: false, label: 'D\'' }   // Down CCW
    ];

    try {
        for (let i = 0; i < sequence.length; i++) {
            const rotation = sequence[i];
            
            // Add to history
            addToHistory(rotation.label);
            
            // Perform rotation (showAlert=false since we handle errors at sequence level)
            await performRotation(rotation.side, rotation.clockwise, false);
            
            // Wait 1 second before next rotation (except for the last one)
            if (i < sequence.length - 1) {
                await new Promise(resolve => setTimeout(resolve, 1000));
            }
        }
    } catch (error) {
        console.error('Error in test sequence:', error);
        alert('Error during test sequence: ' + error.message);
    }
}
